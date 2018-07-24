using PetsPetsPets.Models;
using System.Collections.Generic;
using System.Linq;

namespace PetsPetsPets.TempDataStorage
{
	public class TempUserCollectionStorage
	{
		public TempUserCollectionStorage()
		{
			Users = new Dictionary<string, User>();
		}

		private Dictionary<string, User> Users;

		public User GetUserFromCollection(string userId) => Users.FirstOrDefault(x => x.Value.Id == userId).Value;

		public void AddorUpdateUser(User user)
		{
			Users[user.Id] = user;
		}
	}
}
