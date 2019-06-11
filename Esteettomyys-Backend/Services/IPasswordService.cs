using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Esteettomyys_Backend
{
	public interface IPasswordService
	{
		string EncryptPassword (string plainText);
		bool CheckPassword (string candidate, string encrypted);
	}
}
