using PetsPetsPets.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetsPetsPets.Models
{
    public class Pet
    {
		// create a new pet, calculate current raties to be 50% of the capacity of a rating
		// First iteration of this ctor hardcoded LastFed & LastStroked to DateTime.Now. However, I figured that perhaps more difficult to manage pets might be implemented where  
		public Pet(string name, PetType type, double appetiteCapacity, double happinessCapacity, double appetiteDecayRate, double happinessDecayRate, string ownerId)
		{
			Id = name.Replace(" ", "");
			Type = type;

			AppetiteCapacity = appetiteCapacity;
			CurrentAppetiteRating = appetiteCapacity / 2;

			HappinessCapacity = happinessCapacity;
			CurrentHappinessRating = happinessCapacity / 2;

			AppetiteDecayRate = appetiteDecayRate;
			HappinessDecayRate = happinessDecayRate;

			LastFed = SystemTime.Now();
			LastStroked = SystemTime.Now();

			OwnerId = ownerId;
		}

		public string Id { get; private set; }

		public PetType Type { get; private set; }

		public double AppetiteCapacity { get; private set; } // maximum amount of food a pet can be fed

		public double HappinessCapacity { get; private set; } // maximum happpiness a pet can have

		public double AppetiteDecayRate { get; private set; } // rate in which a pet's appetite will decay over time

		public double HappinessDecayRate { get; private set; } // rate in which a pet's happiness will decay over time

		public DateTime LastFed { get; private set; } // time which to determine the starting point from which the appetite will decay

		public DateTime  LastStroked { get; private set; } // time which to determine the starting point from which the happiness will decay

		public double CurrentAppetiteRating { get; private set; }

		public double CurrentHappinessRating { get; private set; }

		public string OwnerId { get; set; } // userId of the pet's owner

		public void IncrementHappiness(double rating)
		{
			CurrentHappinessRating += rating;
		}

		public void IncrementAppetite(double rating)
		{
			CurrentAppetiteRating += rating;
		}

		public void ApplyAppetiteDecay(double decay)
		{
			CurrentAppetiteRating -= decay;
		}

		public void ApplyHappinessDecay(double decay)
		{
			CurrentHappinessRating -= decay;
		}


		public void UpdateLastFedTime()
		{
			LastFed = SystemTime.Now();
		}

		public void UpdateLastStrokedTime()
		{
			LastStroked = SystemTime.Now();
		}

		public void UpdateOwnership(string newOwnerid) // Extension where perhaps pets can be traded between different users, an API call could be made to update the ownership of said pet
		{
			OwnerId = newOwnerid;
		}

	}
}
