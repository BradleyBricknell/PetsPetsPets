using System;

namespace PetsPetsPets.Helpers
{
	/*
	 * The purpose of this Wrapper class is open up dateTime to be "mocked" so that the decay can be tested.
	 */
	public static class SystemTime
	{
		public static Func<DateTime> Now = () => DateTime.Now;

		public static void SetDateTime(DateTime dateTimeNow)
		{
			Now = () => dateTimeNow;
		}

		public static void ResetDateTime()
		{
			Now = () => DateTime.Now;
		}
	}
}
