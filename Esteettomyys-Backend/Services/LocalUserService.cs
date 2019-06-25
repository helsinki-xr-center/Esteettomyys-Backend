using CryptSharp.Utility;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Esteettomyys_Backend
{
	public class LocalUserService : IUserService
	{
		private List<User> users = new List<User>();

		public LocalUserService (IConfiguration config, IPasswordService passwordService) {

			User testUser1 = new User { username = "testimake1", encryptedPassword = passwordService.EncryptPassword("password"), timeCreated = DateTime.Now, role = UserRole.Student };
			User testUser2 = new User { username = "testimake2", encryptedPassword = passwordService.EncryptPassword("password"), timeCreated = DateTime.Now, role = UserRole.Student };
			User testUser3 = new User { username = "kissa", encryptedPassword = passwordService.EncryptPassword("password"), timeCreated = DateTime.Now, role = UserRole.Teacher };

			_ = Create(testUser1);
			_ = Create(testUser2);
			_ = Create(testUser3);
		}

		/**
		* <summary>
		* Inserts the User into the database.
		* </summary>
		*/
		public async Task Create (User user) {
			await Task.CompletedTask;
			users.Add(user);
		}

		/**
		* <summary>
		* Finds a user that matches the given username
		* </summary>
		*/
		public async Task<User> GetByUsername (string username) {
			await Task.CompletedTask;
			return
			(
			from user in users
			where user.username == username
			select user
			)
			.FirstOrDefault();
		}

		/**
		* <summary>
		* Finds all users from the database.
		* </summary>
		*/
		public IEnumerable<User> GetAll () {
			return users.AsEnumerable();
		}

		/**
		* <summary>
		* Updates the password for a player. newPassword should be already encrypted.
		* </summary>
		*/
		public async Task UpdatePassword (string username, string newPassword) {
			User user = await GetByUsername(username);
			if(user != null) {
				user.encryptedPassword = newPassword;
			}
		}

		/**
		* <summary>
		* Updates the SaveData for a user.
		* </summary>
		*/
		public async Task UpdateSaveData (string username, SaveData newData) {
			User user = await GetByUsername(username);
			if (user != null) {
				user.saveData = newData;
			}
		}

		/**
		* <summary>
		* Returns true if a user with this username exists.
		* </summary>
		*/
		public async Task<bool> UsernameExists (string username) {
			User user = await GetByUsername(username);
			return user != null;
		}
	}
}
