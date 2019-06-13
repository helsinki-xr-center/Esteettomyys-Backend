using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace Esteettomyys_Backend.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class LoginController : ControllerBase
	{

		private UserService userService;
		private IPasswordService passwordService;
		private AuthorizationService authorization;

		public LoginController (UserService service, IPasswordService passwords, AuthorizationService auth) {
			userService = service;
			passwordService = passwords;
			authorization = auth;
		}


		/**
		 * GET/POST api/login
		 * 
		 * <summary>
		 * Takes in the user credentials from Json body with the keys 'username' and 'password'. 
		 * Returns a valid api key as a <see cref="JsonResult"/> if the credentials were correct.
		 * Otherwise returns <see cref="UnauthorizedResult"/>.
		 * </summary>
		 */
		[HttpGet]
		[HttpPost]
		public async Task<IActionResult> Login(JObject credentials)
		{
			if (!credentials.TryGetValue("username", StringComparison.InvariantCultureIgnoreCase, out JToken username) ||
				!credentials.TryGetValue("password", StringComparison.InvariantCultureIgnoreCase, out JToken password)) {
				return BadRequest();
			}

			string realUsername = username.Value<string>().ToLower();

			User user = await userService.GetByUsername(realUsername);

			if(user == null) {
				return Unauthorized();
			}

			if (passwordService.CheckPassword(password.Value<string>(), user.encryptedPassword)) {
				var result = new JsonResult(authorization.GenerateToken(user.username));
				result.StatusCode = StatusCodes.Status200OK;
				return result;
			}

			return Unauthorized();
		}
	}
}
