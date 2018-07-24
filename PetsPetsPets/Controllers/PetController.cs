using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using PetsPetsPets.DataAccess;
using PetsPetsPets.Models;

namespace PetsPetsPets.Controllers
{
	[Route("api")]
	public class PetController : Controller
	{
		private PetDataAccess petDataAccess;

		public PetController()
		{
			/// this in the real implementation the parametre of the mongocollection would consist of a new instance of IDataAccess 
			petDataAccess = new PetDataAccess(new TempDataStorage.TempPetCollectionStorage());
		}

		// GET api/values
		[HttpGet]
		[Route("Pet/{id}/{getPetsFromUserId}")]
		public List<Pet> Get(string id, bool getPetsFromUserId)
		{		
			return getPetsFromUserId ?petDataAccess.FindAllByUserId(id) : new List<Pet>() { petDataAccess.GetPetStatus(id) };	
		}

        // POST api/feed
        [HttpPatch]		
		public void Feed([FromBody] Pet pet, double feedIncrease)
        {
			petDataAccess.FeedThePet(pet, feedIncrease);
        }

		// POST api/stroke
		[HttpPatch]
		public void Stroke([FromBody] Pet pet, double happinessIncrease)
		{
			petDataAccess.StrokeThePet(pet, happinessIncrease);
		}

		// PUT api/save
		[HttpPost]
        public void Save(Pet pet)
        {
			petDataAccess.SavePetStatus(pet);
        }
    }
}
