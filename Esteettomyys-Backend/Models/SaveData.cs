using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Esteettomyys_Backend
{
	/**
	* <summary>
	* Database model for save data.
	* </summary>
	*/
	public class SaveData
	{
		public string saveName;
		public DateTime timeStamp;
		public string data;
	}
}
