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
	public class UserService
	{
		private readonly IMongoCollection<User> users;

		public UserService (IConfiguration config) {
			var client = new MongoClient(config.GetValue<string>("mongodb_connection"));
			var database = client.GetDatabase(config.GetValue<string>("mongodb_database"));
			users = database.GetCollection<User>(config.GetValue<string>("mongodb_collection_users"));

			if (!users.Indexes.List().Any()) {
				var userBuilder = Builders<User>.IndexKeys;
				var indexModel = new CreateIndexModel<User>(userBuilder.Hashed(x => x.username));
				Task.Run(() => users.Indexes.CreateOneAsync(indexModel));
			}
		}

		/**
		* <summary>
		* Inserts the User into the database.
		* </summary>
		*/
		public async Task Create (User user) {
			await users.InsertOneAsync(user);
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
			from user in users.AsQueryable()
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
			return users.AsQueryable();
		}

		/**
		* <summary>
		* Updates the password for a player. newPassword should be already encrypted.
		* </summary>
		*/
		public async Task UpdatePassword (string username, string newPassword) {
			var filter = Builders<User>.Filter.Eq(nameof(User.username), username);
			var update = Builders<User>.Update.Set(nameof(User.encryptedPassword), newPassword);
			await users.UpdateOneAsync(filter, update);
		}

		/**
		* <summary>
		* Updates the SaveData for a user.
		* </summary>
		*/
		public async Task UpdateSaveData (string username, SaveData newData) {
			var filter = Builders<User>.Filter.Eq(nameof(User.username), username);
			var update = Builders<User>.Update.Set(nameof(User.saveData), newData);
			await users.UpdateOneAsync(filter, update);
		}

		/**
		* <summary>
		* Returns true if a user with this username exists.
		* </summary>
		*/
		public async Task<bool> UsernameExists(string username) {
			var filter = Builders<User>.Filter.Eq(nameof(User.username), username);
			var result = await users.FindAsync(filter);
			return result.Any();
		}
	}
}
