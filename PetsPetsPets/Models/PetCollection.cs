using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetsPetsPets.Models
{
    public class PetCollection
    {
		public PetCollection(string userId)
		{
			Id =  $"{userId}'s Collection#{new Random().Next(1, 999)}";
		}

		public string Id { get; private set; }

		public int CollectiveMood { get; } // this is a potential extension property. Perhaps the mood of the pets could affect the mood of other pets somehow. This property could be calculated at runtime rather than storing a fixed value.

		public List<Pet> Pets { get; }
     }
}
