using NUnit.Framework;
using PetsPetsPets.Controllers;
using PetsPetsPets.Models;
using FluentAssertions;
using System.Collections.Generic;

namespace PetsPetsPets.Test
{
	[TestFixture]
    public class ControllerTests
    {
		private PetController petController;
		private Pet petOne;
		private Pet petTwo;
		private Pet petThree;

		[SetUp]
		public void Setup()
		{
			petController = new PetController();
			petOne = new Pet("Iggy The Lizard", PetType.Reptile, 100, 150, 2.5, 2.5, "UserABC123");
			petThree = new Pet("Sammy the Stegosaurus", PetType.Dinosaur, 1000, 50, 100, 1, "UserCDE456");
		}

		[Test]
		public void AsserThatWhenRequestingTheStatusOnAPetTheControllerReturnsTheCorrectDetails()
		{
			petController.Save(petOne);

			petController.Get("IggyTheLizard", false).Should().BeEquivalentTo(new List<Pet> { petOne }); 
		}

		[Test]
		public void AssertWhenRequestingAllPetsThatAnOwnerhasReturnAListOfAllPetsRelatedToTheOwner()
		{
			petController.Save(petOne);
			petController.Save(petThree);

			petController.Get("UserABC123", true).Should().BeEquivalentTo(new List<Pet> { petOne });
		}	
	}
}
