using System;

namespace Consumables.Exceptions
{
    public class PasswordContainsUsernameException : Exception
    {
        private const string ErrorDescription = "Password contains the username";

        public PasswordContainsUsernameException() : base(ErrorDescription)
        {
        }
    }
}