using System.Collections.Generic;
using System.Threading.Tasks;

namespace Esteettomyys_Backend
{
	public interface IUserService
	{
		Task Create (User user);
		IEnumerable<User> GetAll ();
		Task<User> GetByUsername (string username);
		Task UpdatePassword (string username, string newPassword);
		Task UpdateSaveData (string username, SaveData newData);
		Task<bool> UsernameExists (string username);
	}
}