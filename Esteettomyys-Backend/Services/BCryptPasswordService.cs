using CryptSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Esteettomyys_Backend
{
	public class BCryptPasswordService : IPasswordService
	{
		public bool CheckPassword (string candidate, string encrypted) {
			return Crypter.CheckPassword(candidate, encrypted);
		}

		public string EncryptPassword (string plainText) {
			return Crypter.Blowfish.Crypt(plainText);
		}
	}
}
