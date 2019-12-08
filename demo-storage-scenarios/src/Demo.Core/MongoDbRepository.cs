using System;
using System.Collections.Generic;
using Demo.Data;
using MongoDB.Driver;

namespace Demo.Core
{
    public class MongoDbRepository
    {
        public readonly MongoClient Connection;
        private const string ConnectionString = "mongodb://127.0.0.1:27017/TestDB";
        private const string DatabaseName = "TestDB";
        private const string CollectionName = "Users";
        private IClientSessionHandle _session;

        public MongoDbRepository()
        {
            Connection = new MongoClient(ConnectionString);
        }

        public void SetSession(IClientSessionHandle session)
        {
            _session = session;
        }

        public void InsertUser(UserEntity user)
        {
            var collection = GetUserCollection();
            if(_session !=null)
                collection.InsertOne(_session, user);
            else
                collection.InsertOne(user);
        }

        public void InsertUsers(IEnumerable<UserEntity> users)
        {
            var collection = GetUserCollection();
            collection.InsertMany(users);
        }
        
        public List<UserEntity> GetAllUsers()
        {
            var collection = GetUserCollection();

            return collection.Find(_ => true).ToList();
        }

        public List<UserEntity> FindUsers(int? id = null, string code = null)
        {
            if (!id.HasValue && string.IsNullOrEmpty(code))
                throw new ArgumentNullException("must have one parameter.");

            var collection = GetUserCollection();
            IFindFluent<UserEntity, UserEntity> query = null;
            if (id.HasValue)
                query = collection.Find(u => u.Id == id);
            if (!string.IsNullOrEmpty(code))
                query = collection.Find(u => u.Code == code);
            return query.ToList();
        }

        public void DropUserCollection()
        {
            var database = GetDatabase();
            database.DropCollection(CollectionName);
        }

        private IMongoDatabase GetDatabase() => _session != null ? _session.Client.GetDatabase(DatabaseName) : Connection.GetDatabase(DatabaseName);

        private IMongoCollection<UserEntity> GetUserCollection() =>
            GetDatabase().GetCollection<UserEntity>(CollectionName);
    }
}