using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Consumables.Exceptions;
using Consumables.Interfaces;
using Contexts;
using SharedModels;

namespace Consumables
{
    public interface IUserConsumable : IStandardConsumable<UserModel>
    {
    }

    public class UserConsumable : IUserConsumable
    {
        private readonly IUserContext _userContext;

        public UserConsumable(IUserContext userContext)
        {
            _userContext = userContext;
        }

        public async Task<IEnumerable<UserModel>> GetAllAsync()
        {
            return (await _userContext.GetAllAsync()).ToList();
        }

        public async Task<UserModel> GetByIdAsync(string id)
        {
            return await _userContext.GetByIdAsync(id);
        }

        public async Task UpdateAsync(string id, UserModel model)
        {
            if (UserValidation.IsUsernameTaken(model.Username, await GetByIdAsync(id), await GetAllAsync()))
                throw new AlreadyInUseException(model.Username);

            if (UserValidation.PasswordContainsUsername(model.Username, model.Password))
                throw new PasswordContainsUsernameException();

            await _userContext.UpdateAsync(id, model);
        }

        public async Task CreateAsync(UserModel model)
        {
            if (UserValidation.IsUsernameTaken(model.Username, await GetAllAsync()))
                throw new AlreadyInUseException(model.Username);

            if (UserValidation.PasswordContainsUsername(model.Username, model.Password))
                throw new PasswordContainsUsernameException();

            await _userContext.CreateAsync(model);
        }

        public async Task DeleteAsync(string id)
        {
            await _userContext.DeleteAsync(id);
        }
    }
}