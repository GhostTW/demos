using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using Demo.Core;
using Demo.Data;
using MySql.Data.MySqlClient;
using NUnit.Framework;

namespace Demo.UnitTest
{
    public class MariaDbRepositoryUnitTests
    {
        private const string ConnectionString =
            "server=127.0.0.1;port=3326;user id=root;password=pass.123;database=TestDB;charset=utf8;";

        private const string ConnectionString1 =
            "server=127.0.0.1;port=3326;user id=root;password=pass.123;database=TestDB1;charset=utf8;";

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void GetUserShouldCorrect()
        {
            // arrange
            var sut = new MariaDbRepository();

            // action
            var result = sut.GetUsers();
            var firstUser = result.FirstOrDefault();

            // assert
            Assert.IsNotNull(firstUser);
            Assert.AreEqual("Admin", firstUser.Code);
        }

        [Test]
        public void CreateUserShouldCorrect()
        {
            // arrange
            var newUser = new UserEntity {Code = "FromUnitTest", Password = "pass.123", IsActive = false};

            // action
            var sut = new MariaDbRepository();
            var insertedUser = sut.CreateUser(newUser);

            // assert
            Assert.NotZero(newUser.Id);
            Assert.NotZero(insertedUser.Id);
        }

        [Test]
        public void UpdateUserShouldCorrect()
        {
            // arrange
            var newUser = new UserEntity {Id = 3, Password = "pass.123", IsActive = true};

            // action
            var sut = new MariaDbRepository();

            // assert
            Assert.DoesNotThrow(() => sut.UpdateUser(newUser));
        }

        [Test]
        public void GetProductShouldCorrect()
        {
            // arrange
            var sut = new MariaDbRepository();

            // action
            var result = sut.GetProducts();
            var firstProduct = result.FirstOrDefault();

            // assert
            Assert.IsNull(firstProduct);
        }

        [Test]
        public void CreateProductShouldCorrect()
        {
            // arrange
            var newProduct = new ProductEntity {Name = nameof(CreateProductShouldCorrect), Amount = 10, AccountId = 2};

            // action
            var sut = new MariaDbRepository();
            var insertedProduct = sut.CreateProduct(newProduct);

            // assert
            Assert.NotZero(newProduct.Id);
            Assert.NotZero(insertedProduct.Id);
        }

        [Test]
        public void GetUserRollbackShouldCorrect()
        {
            // arrange
            UserEntity firstUser;

            // action
            using (var connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    var sut = new MariaDbRepository(connection, transaction);

                    var result = sut.GetUsers();
                    firstUser = result.FirstOrDefault();
                    if (transaction.Connection != null) transaction.Rollback();
                }
            }

            // assert
            Assert.IsNotNull(firstUser);
            Assert.AreEqual("Admin", firstUser.Code);
        }

        [Test]
        public void CreateUserRollbackShouldCorrect()
        {
            // arrange
            var newUser = new UserEntity {Code = "FromUnitTest", Password = "pass.123", IsActive = false};
            UserEntity insertedUser;
            int originUserCount = 0, tempUserCount = 0, finalUserCount = 0;

            // action
            using (var connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    var sut = new MariaDbRepository(connection, transaction);
                    originUserCount = sut.GetUsers().Count();
                    insertedUser = sut.CreateUser(newUser);
                    tempUserCount = sut.GetUsers().Count();

                    if (transaction.Connection != null) transaction.Rollback();
                    finalUserCount = sut.GetUsers().Count();
                }
            }

            // assert
            Assert.NotZero(insertedUser.Id);
            Assert.AreEqual(originUserCount + 1, tempUserCount);
            Assert.AreEqual(originUserCount, finalUserCount);
        }

        [Test]
        public void UpdateUserRollbackShouldCorrect()
        {
            // arrange
            var originPassword = "1E867FA1A3A64AB5E1EE21BD76F05912";
            var expectedPassword = "pass.123";
            var newUser = new UserEntity {Id = 2, Password = expectedPassword, IsActive = true};
            UserEntity originUser = null, tempUser = null, finalUser = null;

            // action
            using (var connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    var sut = new MariaDbRepository(connection, transaction);
                    originUser = sut.GetUsers().FirstOrDefault(u => u.Id == newUser.Id);
                    Assert.DoesNotThrow(() => sut.UpdateUser(newUser));
                    tempUser = sut.GetUsers().FirstOrDefault(u => u.Id == newUser.Id);
                    if (transaction.Connection != null) transaction.Rollback();
                    finalUser = sut.GetUsers().FirstOrDefault(u => u.Id == newUser.Id);
                }
            }

            // assert
            Assert.AreEqual(originPassword, originUser.Password);
            Assert.AreEqual(expectedPassword, tempUser.Password);
            Assert.AreEqual(originUser.Password, finalUser.Password);
        }

        [Test]
        public void UpdateUserReadDirtyData()
        {
            // arrange
            var originPassword = "1E867FA1A3A64AB5E1EE21BD76F05912";
            var expectedPassword = "pass.123";
            var newUser = new UserEntity {Id = 2, Password = expectedPassword, IsActive = true};
            var dirtyPassword = string.Empty;
            var finalPassword = string.Empty;

            // action
            var waiterUpdate = new AutoResetEvent(false);

            Task.Factory.StartNew(() =>
            {
                waiterUpdate.WaitOne();
                using (var suppressedScope = new TransactionScope(TransactionScopeOption.Required,
                    new TransactionOptions {IsolationLevel = IsolationLevel.ReadUncommitted}))
                {
                    var userRepoB = new MariaDbRepository();
                    dirtyPassword = userRepoB.GetUsers().FirstOrDefault(u => u.Id == 2)?.Password;
                    suppressedScope.Complete();
                }

                waiterUpdate.Set();
            });

            using (var transactionScope = new TransactionScope(TransactionScopeOption.Required,
                new TransactionOptions {IsolationLevel = IsolationLevel.ReadCommitted},
                TransactionScopeAsyncFlowOption.Enabled))
            {
                var userRepoA = new MariaDbRepository();
                userRepoA.UpdateUser(newUser);
                waiterUpdate.Set();
                waiterUpdate.Reset();
                waiterUpdate.WaitOne();

                transactionScope.Dispose();
                finalPassword = userRepoA.GetUsers().FirstOrDefault(u => u.Id == 2)?.Password;
            }

            // assert
            Assert.AreEqual(newUser.Password, dirtyPassword);
            Assert.AreEqual(originPassword, finalPassword);
        }

        [Test]
        public void ReadDirtyDataWithNestedScope()
        {
            // arrange
            var originPassword = "1E867FA1A3A64AB5E1EE21BD76F05912";
            var expectedPassword = "pass.123";
            var newUser = new UserEntity {Id = 2, Password = expectedPassword, IsActive = true};
            var dirtyPassword = string.Empty;
            var finalPassword = string.Empty;

            // action
            using (var transactionScope = new TransactionScope(TransactionScopeOption.RequiresNew,
                new TransactionOptions {IsolationLevel = IsolationLevel.ReadCommitted},
                TransactionScopeAsyncFlowOption.Enabled))
            {
                var userRepoA = new MariaDbRepository();
                userRepoA.UpdateUser(newUser);

                using (var suppressedScope = new TransactionScope(
                    TransactionScopeOption.RequiresNew, // can't use Suppress or Required
                    new TransactionOptions {IsolationLevel = IsolationLevel.ReadUncommitted}))
                {
                    var userRepoB = new MariaDbRepository();
                    dirtyPassword = userRepoB.GetUsers().FirstOrDefault(u => u.Id == 2)?.Password;
                    suppressedScope.Complete();
                }

                transactionScope.Dispose();
                finalPassword = userRepoA.GetUsers().FirstOrDefault(u => u.Id == 2)?.Password;
            }

            // assert
            Assert.AreEqual(newUser.Password, dirtyPassword);
            Assert.AreEqual(originPassword, finalPassword);
        }

        [Test]
        public void NestedScopeInnerScopeRollbackShouldRollback()
        {
            // arrange
            var originPassword = "1E867FA1A3A64AB5E1EE21BD76F05912";
            var expectedPassword = "pass.123";
            var newUser2 = new UserEntity {Id = 2, Password = expectedPassword, IsActive = true};
            var newUser3 = new UserEntity {Id = 3, Password = expectedPassword, IsActive = true};
            var finalPasswordUser2 = string.Empty;
            var finalPasswordUser3 = string.Empty;

            // action
            try
            {
                using (var transactionScope = new TransactionScope(TransactionScopeOption.Required,
                    new TransactionOptions {IsolationLevel = IsolationLevel.ReadCommitted},
                    TransactionScopeAsyncFlowOption.Enabled))
                {
                    var userRepoA = new MariaDbRepository();
                    userRepoA.UpdateUser(newUser2);

                    using (var unused = new TransactionScope(TransactionScopeOption.Required))
                    {
                        var userRepoB = new MariaDbRepository();
                        userRepoB.UpdateUser(newUser3);

                        //unused.Complete();
                    }

                    transactionScope.Complete();
                }
            }
            catch (TransactionAbortedException)
            {
                Console.WriteLine("transaction rollback!");
            }
            var repo = new MariaDbRepository();
            var users = repo.GetUsers().Where(u => u.Id == 2 || u.Id == 3).ToArray();

            finalPasswordUser2 = users.FirstOrDefault(u => u.Id == 2)?.Password;
            finalPasswordUser3 = users.FirstOrDefault(u => u.Id == 3)?.Password;
            // assert
            Assert.AreEqual(originPassword, finalPasswordUser2);
            Assert.AreEqual(originPassword, finalPasswordUser3);
        }

        [Test]
        public void InsertUserAndProductRollbackInTransactionScope()
        {
            // arrange
            var newUser = new UserEntity {Code = "UnitTest_Scope", Password = "pass.123", IsActive = false};
            var newProduct = new ProductEntity {Name = nameof(CreateProductShouldCorrect), Amount = 10, AccountId = 2};
            UserEntity insertedUser;
            ProductEntity insertedProduct;
            int originUserCount = 0, tempUserCount = 0, finalUserCount = 0;
            int originProductCount = 0, tempProductCount = 0, finalProductCount = 0;

            // action
            using (var transactionScope = new TransactionScope(TransactionScopeOption.Required,
                new TransactionOptions {IsolationLevel = IsolationLevel.RepeatableRead},
                TransactionScopeAsyncFlowOption.Enabled))
            {
                var userRepo = new MariaDbRepository();
                var productRepo = new MariaDbRepository();

                originUserCount = userRepo.GetUsers().Count();
                insertedUser = userRepo.CreateUser(newUser);
                tempUserCount = userRepo.GetUsers().Count();

                originProductCount = productRepo.GetProducts().Count();
                insertedProduct = productRepo.CreateProduct(newProduct);
                tempProductCount = productRepo.GetProducts().Count();

                transactionScope.Dispose();
                finalUserCount = userRepo.GetUsers().Count();
                finalProductCount = productRepo.GetProducts().Count();
            }

            // assert
            Assert.NotZero(insertedUser.Id);
            Assert.AreEqual(originUserCount + 1, tempUserCount);
            Assert.AreEqual(originUserCount, finalUserCount);

            Assert.NotZero(insertedProduct.Id);
            Assert.AreEqual(originProductCount + 1, tempProductCount);
            Assert.AreEqual(originProductCount, finalProductCount);
        }
    }
}