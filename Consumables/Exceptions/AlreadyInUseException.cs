using System;

namespace Consumables.Exceptions
{
    public class AlreadyInUseException : Exception
    {
        private const string ErrorDescription = "The value is already in use: ";

        public AlreadyInUseException(string nameOfItemInUse) : base(ErrorDescription + nameOfItemInUse)
        {
        }
    }
}