using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DatabaseAccess.Interfaces;
using Microsoft.Extensions.Configuration;
using Models;
using Models.Base;
using MongoDB.Driver;

namespace DatabaseAccess
{
    public class MongoDatabaseAccess<TEntity> : IDatabaseAccess<TEntity> where TEntity : EntityBase
    {
        protected IMongoDatabase MongoDatabase;
        protected ConnectionModel ConnectionModel;
        protected IMongoCollection<TEntity> Items;

        protected MongoDatabaseAccess(IConfiguration configuration)
        {
            ConnectionModel = new ConnectionModel
            {
                ConnectionString = configuration["ConnectionStrings:UsersDatabaseServer"],
                DatabaseName = configuration["DatabaseNames:Users"],
                CollectionName = configuration["CollectionNames:user"]
            };

            SetupConnectionAsync(configuration);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await CheckExceptions(async () => (await Items.FindAsync(item => true)).ToList());
        }

        public async Task<TEntity> GetByIdAsync(string id)
        {
            return await CheckExceptions(async () => (await Items.FindAsync(item => item.Id == id)).FirstOrDefault());
        }

        public async Task UpdateAsync(string id, TEntity entity)
        {
            await CheckExceptions(async () => await Items.ReplaceOneAsync(item => item.Id == id, entity));
        }

        public async Task CreateAsync(TEntity entity)
        {
            await CheckExceptions(async () => await Items.InsertOneAsync(entity));
        }

        public async Task DeleteAsync(string id)
        {
            await CheckExceptions(async () => await Items.DeleteOneAsync(item => item.Id == id));
        }

        private void SetupConnectionAsync(IConfiguration configuration)
        {
            try
            {
                var mongoClient = new MongoClient(ConnectionModel.ConnectionString);
                MongoDatabase = mongoClient.GetDatabase(ConnectionModel.DatabaseName);
            }
            catch (NullReferenceException nullReferenceException)
            {
                throw new NullReferenceException("Unable to connect to the database", nullReferenceException);
            }
            catch (ArgumentNullException argumentNullException)
            {
                throw new ArgumentNullException("Unable to connect to the database", argumentNullException);
            }
            catch (MongoClientException mongoClientException)
            {
                throw new MongoClientException("Unable to connect to the database", mongoClientException);
            }
            catch (Exception exception)
            {
                throw new Exception("Unknown exception has occurred", exception);
            }
        }

        private static T CheckExceptions<T>(Func<T> func)
        {
            try
            {
                return func();
            }
            catch (ArgumentNullException argumentNullException)
            {
                throw new ArgumentNullException("Unable to connect to the database", argumentNullException);
            }
            catch (TimeoutException timeoutException)
            {
                throw new TimeoutException("Unable to connect to the database", timeoutException);
            }
            catch (Exception exception)
            {
                throw new Exception("Unknown exception has occurred", exception);
            }
        }
    }
}