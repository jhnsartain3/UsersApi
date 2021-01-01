using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Consumables;
using Sartain_Studios_Common.Cryptography;
using Sartain_Studios_Common.Logging;
using Services.Exceptions;
using Services.Interfaces;
using SharedModels;

namespace Services
{
    public interface IUserService : IStandardDataAccess<UserModel>
    {
        Task<bool> UsernameExistsAsync(string username);
        Task<bool> AreCredentialsValidAsync(UserModel userModel);
        Task<int> GetQuantityOfUsersAsync();
        Task<UserModel> GetUserIdFromUsername(string username);
    }

    public class UserService : IUserService
    {
        private readonly IHasher _hasher;
        private readonly ILoggerWrapper _loggerWrapper;
        private readonly IUserConsumable _userConsumable;

        public UserService(ILoggerWrapper loggerWrapper, IUserConsumable userConsumable, IHasher hasher)
        {
            _userConsumable = userConsumable;
            _hasher = hasher;
            _loggerWrapper = loggerWrapper;
        }

        public async Task<IEnumerable<UserModel>> GetAllAsync()
        {
            _loggerWrapper.LogInformation("Get All Users Models", GetType().Name, nameof(GetAllAsync), null);

            return (await _userConsumable.GetAllAsync()).ToList();
        }

        public async Task<UserModel> GetByIdAsync(string id)
        {
            _loggerWrapper.LogInformation($"Get User Model By Id: {id}", GetType().Name, nameof(GetByIdAsync), null);

            return await _userConsumable.GetByIdAsync(id);
        }

        public async Task UpdateAsync(string id, UserModel model)
        {
            _loggerWrapper.LogInformation($"Update User Model: {id}", GetType().Name, nameof(UpdateAsync), null);

            model.Password = _hasher.GenerateHash(model.Password);

            await _userConsumable.UpdateAsync(id, model);
        }

        public async Task CreateAsync(UserModel model)
        {
            _loggerWrapper.LogInformation($"Create User Model: {model.Username}", GetType().Name, nameof(CreateAsync),
                null);

            model.Password = _hasher.GenerateHash(model.Password);

            await _userConsumable.CreateAsync(model);
        }

        public async Task DeleteAsync(string id)
        {
            _loggerWrapper.LogInformation($"Delete User Model: {id}", GetType().Name, nameof(DeleteAsync), null);

            await _userConsumable.DeleteAsync(id);
        }

        public async Task<bool> UsernameExistsAsync(string username)
        {
            _loggerWrapper.LogInformation($"User Model exists with name: {username}" + username, GetType().Name,
                nameof(UsernameExistsAsync), null);

            var userModels = await GetAllAsync();

            return userModels.Any(x => x.Username != null && x.Username.Equals(username));
        }

        public async Task<bool> AreCredentialsValidAsync(UserModel userModel)
        {
            _loggerWrapper.LogInformation($"Credentials are valid: {userModel.Username}", GetType().Name,
                nameof(AreCredentialsValidAsync), null);

            var userModels = await GetAllAsync();

            userModel.Password = _hasher.GenerateHash(userModel.Password);

            return userModels.Any(x =>
                x.Username != null && x.Username.Equals(userModel.Username) && x.Password != null &&
                x.Password.Equals(userModel.Password));
        }

        public async Task<int> GetQuantityOfUsersAsync()
        {
            _loggerWrapper.LogInformation("Get quantity of user models", GetType().Name, nameof(GetQuantityOfUsersAsync), null);

            var allUsers = await GetAllAsync();

            return allUsers.Count();
        }

        public async Task<UserModel> GetUserIdFromUsername(string username)
        {
            _loggerWrapper.LogInformation($"Get user id from username: {username}", GetType().Name, nameof(GetUserIdFromUsername), null);

            var userModels = await GetAllAsync();

            if (await UsernameExistsAsync(username))
            {
                var userModel = userModels.Where(x => x.Username != null && x.Username.Equals(username)).Select(x => x).FirstOrDefault();
                return new UserModel { Id = userModel.Id };
            }
            else throw new ItemNotFoundException(username, null);
        }
    }
}