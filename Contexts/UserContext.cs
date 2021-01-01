using DatabaseInteraction;
using DatabaseInteraction.Interfaces;
using DatabaseInteraction.Models;
using Microsoft.Extensions.Configuration;
using SharedModels;

namespace Contexts
{
    public interface IUserContext : IDatabaseAccessWithoutUserId<UserModel>
    {
    }

    public class UserContext : MongoDatabaseAccessWithoutUserId<UserModel>, IUserContext
    {
        public UserContext(IConfiguration configuration) : base(configuration)
        {
            var connectionModel = new ConnectionModel
            {
                ConnectionString = configuration["ConnectionStrings:UsersDatabaseServer"],
                DatabaseName = configuration["DatabaseNames:Users"],
                CollectionName =
                    configuration[$"CollectionNames:{nameof(UserContext).TrimEnd("Context".ToCharArray())}"]
            };

            SetupConnectionAsync(connectionModel);
            
            Items = MongoDatabase.GetCollection<UserModel>(connectionModel.CollectionName);
        }
    }
}