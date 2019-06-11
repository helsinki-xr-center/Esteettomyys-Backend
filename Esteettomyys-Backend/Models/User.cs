using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Esteettomyys_Backend
{
	public class User
	{
		[BsonId]
		public string username { get; set; }
		[BsonElement("Password")]
		public string encryptedPassword { get; set; }
		[BsonElement("SaveData")]
		public SaveData saveData { get; set; }
	}
}
