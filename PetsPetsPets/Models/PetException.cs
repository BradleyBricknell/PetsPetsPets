using System;

namespace PetsPetsPets.Models
{
	public class PetException : Exception
    {
		public PetException(string message)
			: base(message) { }
    }
}
