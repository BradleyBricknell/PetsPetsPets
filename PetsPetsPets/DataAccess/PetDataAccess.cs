using PetsPetsPets.Helpers;
using PetsPetsPets.Models;
using PetsPetsPets.TempDataStorage;
using System;
using System.Collections.Generic;

namespace PetsPetsPets.DataAccess
{
	/*
	 * The dataaccess layer would usually be used to communicate with external databases and collections. For this exercise I am mocking a database call by referencing my TempDataStorage classes.
	 */
    public class PetDataAccess
    {
		private TempPetCollectionStorage PetDataStorage;

		// In a real inmplementation this constructure would take an IMongoCollection
		public PetDataAccess(TempPetCollectionStorage petDataStorage)
		{
			PetDataStorage = new TempPetCollectionStorage();
		}

		public void StrokeThePet(Pet pet, double ratingIncrease)
		{
			if (ratingIncrease < 1)
				throw new PetException("Pet stroke failed, cannot stroke a negative value.");

			if ((pet.CurrentHappinessRating + ratingIncrease) > pet.HappinessCapacity)
				throw new PetException("Pet stroke failed, Pet is happy enough!");

			pet.IncrementHappiness(ratingIncrease);
			pet.UpdateLastStrokedTime();

			SavePetStatus(pet);
		}

		public void FeedThePet(Pet pet, double ratingIncrease)
		{
			if (ratingIncrease < 1)
				throw new PetException("Pet feed failed, cannot feed a negative value.");

			if ((pet.CurrentAppetiteRating + ratingIncrease) > pet.AppetiteCapacity)
				throw new PetException("Pet feed failed, attempt to over feed detected.");

			pet.IncrementAppetite(ratingIncrease);
			pet.UpdateLastFedTime();

			SavePetStatus(pet);
		}

		// save a new pet or update an existing pet
		public void SavePetStatus(Pet pet)
		{
			PetDataStorage.AddorUpdatePet(pet);
		}

		public Pet GetPetStatus(string petId)
		{
			var pet = PetDataStorage.GetPetFromCollection(petId);
		
			return ApplyMetricsDecay(pet);
		}

		public List<Pet> FindAllByUserId(string userId)
		{
			return PetDataStorage.GetAllPetsByUserId(userId);
		}

		private Pet ApplyMetricsDecay(Pet pet)
		 {
			var lastFedhours = (SystemTime.Now() - pet.LastFed).TotalHours;

			pet.ApplyAppetiteDecay(Convert.ToInt32(lastFedhours * pet.AppetiteDecayRate));

			var lastStrokedhours = (SystemTime.Now() - pet.LastStroked).TotalHours;

			pet.ApplyHappinessDecay(Convert.ToInt32(lastStrokedhours * pet.HappinessDecayRate));

			return pet;
		}

		public void TradePet(Pet pet, string newOwner)
		{
			pet.UpdateOwnership(newOwner);
		}
	}
}
