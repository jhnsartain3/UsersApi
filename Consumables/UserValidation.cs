using System.Collections.Generic;
using System.Linq;
using Models;

namespace Consumables
{
    public class UserValidation
    {
        public static bool IsUsernameTaken(string username, UserModel existingUserModel,
            IEnumerable<UserModel> allUserModels)
        {
            return !username.ToLower().Equals(existingUserModel.Username.ToLower()) &&
                   UsernameIsInUse(username, allUserModels);
        }

        public static bool IsUsernameTaken(string username, IEnumerable<UserModel> allUserModels)
        {
            return UsernameIsInUse(username, allUserModels);
        }

        public static bool PasswordContainsUsername(string username, string password)
        {
            return password.ToLower().Contains(username.ToLower());
        }

        private static bool UsernameIsInUse(string username, IEnumerable<UserModel> existingEntity)
        {
            return existingEntity.Any(x => x.Username.ToLower().Equals(username.ToLower()));
        }
    }
}