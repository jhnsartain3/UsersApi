using System;

namespace Services.Exceptions
{
    public class ItemNotFoundException : Exception
    {
        private const string ErrorDescription = " was not found with ID: ";

        public ItemNotFoundException(string nameOfItemInUse="n/a", string id="n/a") : base(nameOfItemInUse + ErrorDescription + id, null)
        {
        }
    }
}