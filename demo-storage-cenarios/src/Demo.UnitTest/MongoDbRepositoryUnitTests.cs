using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using Demo.Core;
using Demo.Data;
using NUnit.Framework;

namespace Demo.UnitTest
{
    public class MongoDbRepositoryUnitTests
    {
        private MongoDbRepository _dbRepository;
        private Faker<UserEntity> _userGenerator;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            // Since transaction is not allowed to create a collection, it's need to create at init.
            new MongoDbRepository().InsertUser(new UserEntity { Id = 0, Code = "create", IsActive = true, Password = "pass.123" });
        }
        [SetUp]
        public void SetUp()
        {
            _dbRepository = new MongoDbRepository();
            _userGenerator = new Faker<UserEntity>()
                                 .RuleFor(u => u.Id, f => f.Random.Int())
                                 .RuleFor(u => u.Code, f => Guid.NewGuid().ToString("N").Substring(0, 8))
                                 .RuleFor(u => u.Password, f => f.Internet.Password())
                                 .RuleFor(u => u.IsActive, f => f.Random.Bool());
        }
        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _dbRepository.DropUserCollection();
        }
        
        [Test]
        public async Task Transaction_InsertUser_should_success()
        {
            var sut = _dbRepository;
            var user = _userGenerator.Generate();

            using (var session = await _dbRepository.Connection.StartSessionAsync())
            {
                sut.SetSession(session);
                session.StartTransaction();

                Assert.DoesNotThrow(() => sut.InsertUser(user));

                await session.CommitTransactionAsync();
            }
            var inserted = sut.FindUsers(user.Id).Single();

            Assert.IsNotNull(inserted);
        }

        [Test]
        public async Task Transaction_InsertUser_should_rollback()
        {
            var sut = _dbRepository;
            var user = _userGenerator.Generate();

            using (var session = await _dbRepository.Connection.StartSessionAsync())
            {
                sut.SetSession(session);
                session.StartTransaction();

                Assert.DoesNotThrow(() => sut.InsertUser(user));

                await session.AbortTransactionAsync();
            }
            var inserted = sut.FindUsers(user.Id).Any();

            Assert.IsFalse(inserted);
        }

        [Test]
        public void InsertUsersShouldSuccess()
        {
            var users = _userGenerator.Generate(2);

            var sut = _dbRepository;
            
            Assert.DoesNotThrow(() => sut.InsertUsers(users));
        }
        
        [Test]
        public void GetAllUsersShouldSuccess()
        {
            IEnumerable<UserEntity> actualResult = null;
            var sut = _dbRepository;

            var users = _userGenerator.Generate(2);
            sut.InsertUsers(users);

            Assert.DoesNotThrow(() => actualResult = sut.GetAllUsers());
            Assert.IsNotEmpty(actualResult);
        }
    }
}