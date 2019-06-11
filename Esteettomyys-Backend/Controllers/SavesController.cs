using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using Newtonsoft.Json.Linq;

namespace Esteettomyys_Backend.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class SavesController : ControllerBase
	{

		private UserService userService;

		public SavesController (UserService service) {
			userService = service;
		}

		// GET api/saves
		[JwtAuthroize]
		[HttpGet]
		public async Task<ActionResult<SaveData>> Get()
		{
			string username = HttpContext.User.GetAuthenticatedUsername();

			User user = await userService.GetByUsername(username);

			SaveData data = user.saveData;

			return data;
		}

		// POST api/saves
		[JwtAuthroize]
		[HttpPost]
		public async Task<IActionResult> Post([FromBody] SaveData newData)
		{
			string username = HttpContext.User.GetAuthenticatedUsername();

			await userService.UpdateSaveData(username, newData);

			return Accepted();
		}

		// GET api/saves/plain
		[JwtAuthroize]
		[HttpGet("plain")]
		public async Task<ActionResult> GetPlain () {
			string username = HttpContext.User.GetAuthenticatedUsername();

			User user = await userService.GetByUsername(username);

			//TODO: Decompress and Jsonify

			return new JsonResult(user.saveData);
		}

	}
}
