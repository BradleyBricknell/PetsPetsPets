using PetsPetsPets.TempDataStorage;

namespace PetsPetsPets.Models
{
    public class User
    {
		public User(string userId)
		{
			Id = userId;
		}

		public string Id { get; private set; }
    }
}
