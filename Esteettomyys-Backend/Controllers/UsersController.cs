using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace Esteettomyys_Backend.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UsersController : ControllerBase
	{
		private UserService userService;
		private IPasswordService passwordService;

		public UsersController(UserService service, IPasswordService passwords) {
			userService = service;
			passwordService = passwords;
		}

		// POST api/<controller>/create
		[HttpPost("create")]
		public async Task<IActionResult> Create (JObject credentials) {
			if (!credentials.TryGetValue("username", StringComparison.InvariantCultureIgnoreCase, out JToken username) ||
				!credentials.TryGetValue("password", StringComparison.InvariantCultureIgnoreCase, out JToken password)) {
				return BadRequest();
			}

			string realUsername = username.Value<string>().ToLower();
			string encryptedPassword = passwordService.EncryptPassword(password.Value<string>());

			User user = new User() {
				username = realUsername,
				encryptedPassword = encryptedPassword
			};

			if(await userService.UsernameExists(realUsername)) {
				return Conflict();
			} else {
				await userService.Create(user);
			}

			return Accepted();
		}

		// DELETE api/<controller>/delete/{username}
		[HttpDelete("delete/{username}")]
		public async Task<IActionResult> Delete (string username) {
			string realUsername = username.ToLower();

			if(!await userService.UsernameExists(username)) {
				return Conflict();
			}

			//TODO: delete users

			return Accepted();
		}
	}
}
