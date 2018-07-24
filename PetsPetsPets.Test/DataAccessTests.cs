using NUnit.Framework;
using FluentAssertions;
using PetsPetsPets.Models;
using PetsPetsPets.TempDataStorage;
using PetsPetsPets.DataAccess;
using System;
using PetsPetsPets.Helpers;
using System.Collections.Generic;

namespace PetsPetsPets
{
	[TestFixture]
	public class DataAccessTests
	{
		private TempPetCollectionStorage petCollection;
		private TempUserCollectionStorage userCollection;

		private PetDataAccess testPetDataAccess;

		private Pet petOne;
		private Pet petTwo;
		private Pet petThree;

		[SetUp]
		public void Setup()
		{
			petCollection = new TempPetCollectionStorage();
			userCollection = new TempUserCollectionStorage();

			testPetDataAccess = new PetDataAccess(petCollection);
			petOne = new Pet("Iggy The Lizard", PetType.Reptile, 100, 150, 2.5, 2.5, "UserABC123");
			petTwo = new Pet("Billy The Boxer", PetType.Dog, 200, 300, 5, 5, "UserABC123");
			petThree = new Pet("Sammy the Stegosaurus", PetType.Dinosaur, 1000, 50, 100, 1, "UserCDE456");

		}

		[Test]
		public void AssertThatWhenAddingAPetToAUserThePetCanInspectedContainingNeutralMetrics()
		{
			testPetDataAccess.SavePetStatus(petOne);
			var returnedPet = testPetDataAccess.GetPetStatus("IggyTheLizard");

			returnedPet.CurrentAppetiteRating.Should().Be(50);
			returnedPet.CurrentHappinessRating.Should().Be(75);
		}

		[Test]
		public void AssertThatWhenAddingAPetThatAlreadyExistsUpdateTheExistingPetInsteadAndAssertThePetCanInspected()
		{
			testPetDataAccess.SavePetStatus(petTwo);
			testPetDataAccess.SavePetStatus(petTwo); // try to add it again

			testPetDataAccess.GetPetStatus("BillyTheBoxer")
				.Should().BeEquivalentTo(petTwo);
		}

		[Test]
		public void AssertThatWhenFeedingAPetThatIsAbleToBeFedTheAppetiteIncreasesAsExpected()
		{
			testPetDataAccess.SavePetStatus(petOne);

			testPetDataAccess.FeedThePet(petOne, 20);

			testPetDataAccess.GetPetStatus("IggyTheLizard").CurrentAppetiteRating.Should().Be(70);			
		}

		[Test]
		public void AssertThatWhenAPetCannotBeFedExpectedTheResponseThatThePetIsFull()
		{
			testPetDataAccess.SavePetStatus(petOne);

			Action overFeedPet = () => testPetDataAccess.FeedThePet(petOne, 100);
			overFeedPet.Should().Throw<PetException>().WithMessage("Pet feed failed, attempt to over feed detected.");			
		}

		[Test]
		public void AssertThatAfterAnAppropriateAmountofTimeHasPassedTheAppetiteOfThePetHasDecayedAccordingly()
		{
			testPetDataAccess.SavePetStatus(petOne);

			SystemTime.SetDateTime(DateTime.Now.AddHours(10)); // mock a day passing so the decay can be calcuated

			testPetDataAccess.GetPetStatus(petOne.Id).CurrentAppetiteRating.Should().Be(25);

			SystemTime.ResetDateTime();
		}

		[Test]
		public void AssertThatAttemptingToFeedAValueWithANegativeValueShouldBeHandled()
		{
			testPetDataAccess.SavePetStatus(petOne);

			Action overFeedPet = () => testPetDataAccess.FeedThePet(petOne, -10);
			overFeedPet.Should().Throw<PetException>().WithMessage("Pet feed failed, cannot feed a negative value.");
		}

		[Test]
		public void AssertThatWhenStrokingAPetThatIsAbleToBeStrokedTheHappinessIncreasesAsExpected()
		{
			testPetDataAccess.SavePetStatus(petOne);

			testPetDataAccess.StrokeThePet(petOne, 20);

			testPetDataAccess.GetPetStatus("IggyTheLizard").CurrentHappinessRating.Should().Be(95);
		}

		[Test]
		public void AssertThatWhenApetCannotBeStrokedExpectedTheResponseThatThePetIsHappyEnough()
		{
			testPetDataAccess.SavePetStatus(petOne);

			Action overFeedPet = () => testPetDataAccess.StrokeThePet(petOne, 100);
			overFeedPet.Should().Throw<PetException>().WithMessage("Pet stroke failed, Pet is happy enough!");
		}

		[Test]
		public void AssertThatAfterAnAppropriateAmountofTimeHasPassedTheHappinessOfThePetHasDecayedAccordingly()
		{
			testPetDataAccess.SavePetStatus(petOne);

			SystemTime.SetDateTime(DateTime.Now.AddHours(10)); // mock a day passing so the decay can be calcuated

			testPetDataAccess.GetPetStatus(petOne.Id).CurrentHappinessRating.Should().Be(50);

			SystemTime.ResetDateTime();
		}


		[Test]
		public void AssertThatAttemptingToStrokeAValueWithANegativeValueShouldBeHandled()
		{
			testPetDataAccess.SavePetStatus(petOne);

			Action overFeedPet = () => testPetDataAccess.StrokeThePet(petOne, -10);
			overFeedPet.Should().Throw<PetException>().WithMessage("Pet stroke failed, cannot stroke a negative value.");
		}

		[Test]
		public void AsserThatAUserWithMultiplePetsFeedingPetOneDoesNotAffectPetNumberTwo()
		{
			testPetDataAccess.SavePetStatus(petOne);
			testPetDataAccess.SavePetStatus(petTwo);

			testPetDataAccess.FeedThePet(petOne, 20);

			testPetDataAccess.GetPetStatus("IggyTheLizard").CurrentAppetiteRating.Should().Be(70); // 50 + 20
			testPetDataAccess.GetPetStatus("BillyTheBoxer").CurrentAppetiteRating.Should().Be(100); // 100 + 0
		}

		[Test]
		public void AsserThatAUserWithMultiplePetsStrokingPetOneDoesNotAffectPetNumberTwo()
		{
			testPetDataAccess.SavePetStatus(petOne);
			testPetDataAccess.SavePetStatus(petTwo);

			testPetDataAccess.StrokeThePet(petOne, 20);

			testPetDataAccess.GetPetStatus("IggyTheLizard").CurrentHappinessRating.Should().Be(95); // 75 + 20
			testPetDataAccess.GetPetStatus("BillyTheBoxer").CurrentHappinessRating.Should().Be(150); // 150 + 0
		}

		[Test]
		public void AsserWhenTradingAPetBetweenUsersThePetCollecionsAreUpdated()
		{
			testPetDataAccess.SavePetStatus(petOne);
			testPetDataAccess.SavePetStatus(petTwo);
			testPetDataAccess.SavePetStatus(petThree);

			testPetDataAccess.TradePet(petTwo, "UserCDE456");

			testPetDataAccess.FindAllByUserId("UserCDE456").Should().BeEquivalentTo(new List<Pet> { petTwo, petThree });
		}
	}
}
