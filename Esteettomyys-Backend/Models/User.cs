﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Esteettomyys_Backend
{
	/**
	* <summary>
	* Database model for the User.
	* </summary>
	*/
	public class User
	{
		[BsonId]
		public string username { get; set; }
		[BsonElement("Password")]
		public string encryptedPassword { get; set; }
		[BsonElement("CreationTime")]
		public DateTime timeCreated;
		[BsonElement("Role")]
		public UserRole role;
		[BsonElement("SaveData")]
		public SaveData saveData { get; set; }
	}

	public enum UserRole
	{
		Guest,
		Student,
		Teacher,
		Admin
	}
}
