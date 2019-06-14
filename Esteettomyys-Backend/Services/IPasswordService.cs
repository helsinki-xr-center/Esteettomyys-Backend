using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Esteettomyys_Backend
{
	public interface IPasswordService
	{
		/**
		* <summary>
		* Checks if the candidate and encrypted password match.
		* </summary>
		*/
		string EncryptPassword (string plainText);

		/**
		* <summary>
		* Returns an encrypted version of the plainText password.
		* </summary>
		*/
		bool CheckPassword (string candidate, string encrypted);
	}
}
