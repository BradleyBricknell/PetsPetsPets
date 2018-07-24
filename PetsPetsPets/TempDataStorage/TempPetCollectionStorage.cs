using PetsPetsPets.Models;
using System.Collections.Generic;
using System.Linq;

namespace PetsPetsPets.TempDataStorage
{
	/*
	 * for the purpose of this exercise, i will use in memory storage for the data.
	 * In a real implementation I would consider creating a mongo instance and use a collection for each of the TempDataCollections here. 
	 */
	public class TempPetCollectionStorage
    {
		public TempPetCollectionStorage()
		{
			Pets = new Dictionary<string, Pet>();
		}

		private  Dictionary<string, Pet> Pets;

		public List<Pet> GetAllPetsByUserId(string userId)
		{
			var pets = Pets.Where(x => x.Value.OwnerId == userId).ToDictionary( x=> x.Key, y => y.Value);

			return pets.Values.ToList();
		}

		public Pet GetPetFromCollection(string petName) => Pets.FirstOrDefault(x => x.Value.Id == petName).Value;

		public  void AddorUpdatePet(Pet pet)
		{
			Pets[pet.Id] = pet;
		}
	}
}