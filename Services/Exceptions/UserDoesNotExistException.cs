using System;

namespace Services.Exceptions
{
    public class UserDoesNotExistException : Exception
    {
        private const string ErrorDescription = "The specified user does not exist: ";

        public UserDoesNotExistException(string identifier) : base(ErrorDescription + identifier)
        {
        }
    }
}