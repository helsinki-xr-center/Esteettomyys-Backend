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

		private IUserService userService;

		public SavesController (IUserService service) {
			userService = service;
		}

		/**
		 * GET api/saves
		 * 
		 * <summary>
		 * Returns the SaveData for the authenticated user.
		 * The user needs to be authenticated to call this endpoint.
		 * </summary>
		 */
		[JwtAuthroize]
		[HttpGet]
		public async Task<ActionResult<SaveData>> Get()
		{
			string username = HttpContext.User.GetAuthenticatedUsername();

			User user = await userService.GetByUsername(username);

			SaveData data = user.saveData;

			return data;
		}

		/**
		 * POST api/saves
		 * 
		 * <summary>
		 * Saves the provided SaveData for the authenticated user.
		 * The user needs to be authenticated to call this endpoint.
		 * </summary>
		 */
		[JwtAuthroize]
		[HttpPost]
		public async Task<IActionResult> Post([FromBody] SaveData newData) {
			string username = HttpContext.User.GetAuthenticatedUsername();

			await userService.UpdateSaveData(username, newData);

			return Accepted();
		}

		/**
		 * DELETE api/saves
		 * 
		 * <summary>
		 * Deletes the SaveData for the authenticated user.
		 * The user needs to be authenticated to call this endpoint.
		 * </summary>
		 */
		[JwtAuthroize]
		[HttpDelete]
		public async Task<IActionResult> Delete () {
			string username = HttpContext.User.GetAuthenticatedUsername();

			await userService.UpdateSaveData(username, null);

			return Accepted();
		}

		/**
		 * GET api/saves/plain
		 * 
		 * <summary>
		 * Returns the SaveData as an uncompressed JSON. Use only for debugging.
		 * The user needs to be authenticated to call this endpoint.
		 * </summary>
		 */
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
