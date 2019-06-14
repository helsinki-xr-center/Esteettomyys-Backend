using CryptSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Esteettomyys_Backend
{
	public class BCryptPasswordService : IPasswordService
	{
		/**
		* <summary>
		* Checks if the candidate and encrypted password match.
		* </summary>
		*/
		public bool CheckPassword (string candidate, string encrypted) {
			return Crypter.CheckPassword(candidate, encrypted);
		}

		/**
		* <summary>
		* Returns an encrypted version of the plainText password.
		* </summary>
		*/
		public string EncryptPassword (string plainText) {
			return Crypter.Blowfish.Crypt(plainText);
		}
	}
}
