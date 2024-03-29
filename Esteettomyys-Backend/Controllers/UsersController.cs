﻿using System;
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
		private IUserService userService;
		private IPasswordService passwordService;

		public UsersController(IUserService service, IPasswordService passwords) {
			userService = service;
			passwordService = passwords;
		}

		/**
		 * POST api/users/create
		 * 
		 * <summary>
		 * Creates a new user with the provided credentials.
		 * The credentials should be provided by JSON in the body.
		 * like:
		 * <code>
		 * {
		 *		"username" : "user"
		 *		"password" : "pass"
		 * }
		 * </code>
		 * </summary>
		 */
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
				encryptedPassword = encryptedPassword,
				timeCreated = DateTime.Now,
				role = UserRole.Student
			};

			if(await userService.UsernameExists(realUsername)) {
				return UnprocessableEntity("This username already exists.");
			} else {
				await userService.Create(user);
			}

			return Accepted();
		}

		/**
		 * DELETE api/users/delete/{username}
		 * 
		 * <summary>
		 * Deletes the provided user.
		 * </summary>
		 */
		[HttpDelete("delete/{username}")]
		public async Task<IActionResult> Delete (string username) {
			string realUsername = username.ToLower();

			if(!await userService.UsernameExists(realUsername)) {
				return NotFound();
			}

			//TODO: delete users

			return Accepted();
		}
	}
}
