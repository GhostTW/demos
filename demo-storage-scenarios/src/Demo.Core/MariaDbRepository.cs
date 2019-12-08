using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Demo.Data;
using MySql.Data.MySqlClient;

namespace Demo.Core
{
    public class MariaDbRepository : IDisposable
    {
        private MySqlConnection _connection;
        private MySqlTransaction _transaction;

        private readonly string _connectionString;

        public MariaDbRepository(MySqlConnection mySqlConnection = null, MySqlTransaction transaction = null, string connectionString = null)
        {
            _connection = mySqlConnection;
            _transaction = transaction;
            _connectionString = connectionString ?? "server=127.0.0.1;port=3326;user id=root;password=pass.123;database=TestDB;charset=utf8;";
        }
        
        public IEnumerable<UserEntity> GetUsers()
        {
            IEnumerable<UserEntity> Execute(MySqlConnection conn)
            {
                using (var command = new MySqlCommand(StoredProcedures.SpUserGetUsers, conn))
                {
                    var parameter = new MySqlParameter("@OUT_ReturnValue", MySqlDbType.Int32)
                    {
                        Direction = ParameterDirection.Output
                    };

                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(parameter);
                    var reader = command.ExecuteReader();

                    if (!reader.HasRows) return Enumerable.Empty<UserEntity>();

                    var result = new List<UserEntity>();
                    while (reader.Read())
                    {
                        var id = reader.GetInt32("Id");
                        var code = reader.GetString("Code");
                        var password = reader.GetString("Password");
                        var isActive = reader.GetBoolean("IsActive");
                        var user = new UserEntity {Id = id, Code = code, Password = password, IsActive = isActive};
                        result.Add(user);
                    }

                    return result;
                }
            }
            
            if(_connection == null)
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                    return Execute(connection);
                }
            }
            else
            {
                return Execute(_connection);
            }
        }

        public UserEntity CreateUser(UserEntity user)
        {
            UserEntity Execute(MySqlConnection conn)
            {
                using (var command = new MySqlCommand(StoredProcedures.SpUserCreateUser, conn))
                {
                    
                    var parameters = new List<MySqlParameter>();
                    var parameterOut = new MySqlParameter("@OUT_ReturnValue", MySqlDbType.Int32)
                    {
                        Direction = ParameterDirection.Output
                    };
                    var parameterCode = new MySqlParameter("@IN_Code", MySqlDbType.String)
                    {
                        Direction = ParameterDirection.Input,
                        Value =  user.Code
                    };
                    var parameterPassword = new MySqlParameter("@IN_Password", MySqlDbType.String)
                    {
                        Direction = ParameterDirection.Input,
                        Value = user.Password
                    };
                    var parameterIsActive = new MySqlParameter("@IN_IsActive", MySqlDbType.Bit)
                    {
                        Direction = ParameterDirection.Input,
                        Value = user.IsActive
                    };
                    parameters.Add(parameterOut);
                    parameters.Add(parameterCode);
                    parameters.Add(parameterPassword);
                    parameters.Add(parameterIsActive);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddRange(parameters.ToArray());
                    var reader = command.ExecuteReader();
                    
                    if (!reader.HasRows) return null;

                    while (reader.Read())
                    {
                        var id = reader.GetInt32("Id");
                        user.Id = id;

                        break;
                    }

                    return user;
                }
            }
            
            if(_connection == null)
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                    return Execute(connection);
                }
            }
            else
            {
                return Execute(_connection);
            }
        }

        public void UpdateUser(UserEntity user)
        {
            void Execute(MySqlConnection conn)
            {
                using (var command = new MySqlCommand(StoredProcedures.SpUserUpdateUser, conn))
                {
                    var parameters = new List<MySqlParameter>();
                    var parameterOut = new MySqlParameter("@OUT_ReturnValue", MySqlDbType.Int32)
                    {
                        Direction = ParameterDirection.Output
                    };
                    var parameterId = new MySqlParameter("@IN_Id", MySqlDbType.String)
                    {
                        Direction = ParameterDirection.Input,
                        Value =  user.Id
                    };
                    var parameterPassword = new MySqlParameter("@IN_Password", MySqlDbType.String)
                    {
                        Direction = ParameterDirection.Input,
                        Value =  user.Password
                    };
                    var parameterIsActive = new MySqlParameter("@IN_IsActive", MySqlDbType.Bit)
                    {
                        Direction = ParameterDirection.Input,
                        Value = user.IsActive
                    };
                    parameters.Add(parameterOut);
                    parameters.Add(parameterId);
                    parameters.Add(parameterPassword);
                    parameters.Add(parameterIsActive);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddRange(parameters.ToArray());
                    command.ExecuteNonQuery();
                }
            }
            
            if(_connection == null)
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                    Execute(connection);
                }
            }
            else
            {
                Execute(_connection);
            }
        }
        
        public IEnumerable<ProductEntity> GetProducts()
        {
            IEnumerable<ProductEntity> Execute(MySqlConnection conn)
            {
                using (var command = new MySqlCommand(StoredProcedures.SpProductGetProducts, conn))
                {
                    var parameter = new MySqlParameter("@OUT_ReturnValue", MySqlDbType.Int32)
                    {
                        Direction = ParameterDirection.Output
                    };

                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(parameter);
                    var reader = command.ExecuteReader();

                    if (!reader.HasRows) return Enumerable.Empty<ProductEntity>();

                    var result = new List<ProductEntity>();
                    while (reader.Read())
                    {
                        var id = reader.GetInt32("Id");
                        var name = reader.GetString("Name");
                        var amount = reader.GetInt32("Amount");
                        var accountId = reader.GetInt32("AccountId");
                        var product = new ProductEntity {Id = id, Name = name, Amount = amount, AccountId = accountId};
                        result.Add(product);
                    }

                    return result;
                }
            }
            
            if(_connection == null)
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                    return Execute(connection);
                }
            }
            else
            {
                return Execute(_connection);
            }
        }

        public ProductEntity CreateProduct(ProductEntity product)
        {
            ProductEntity Execute(MySqlConnection conn)
            {
                using (var command = new MySqlCommand(StoredProcedures.SpProductCreateProduct, conn))
                {
                    
                    var parameters = new List<MySqlParameter>();
                    var parameterOut = new MySqlParameter("@OUT_ReturnValue", MySqlDbType.Int32)
                    {
                        Direction = ParameterDirection.Output
                    };
                    var parameterCode = new MySqlParameter("@IN_Name", MySqlDbType.String)
                    {
                        Direction = ParameterDirection.Input,
                        Value =  product.Name
                    };
                    var parameterPassword = new MySqlParameter("@IN_Amount", MySqlDbType.String)
                    {
                        Direction = ParameterDirection.Input,
                        Value = product.Amount
                    };
                    var parameterIsActive = new MySqlParameter("@IN_AccountId", MySqlDbType.Bit)
                    {
                        Direction = ParameterDirection.Input,
                        Value = product.AccountId
                    };
                    parameters.Add(parameterOut);
                    parameters.Add(parameterCode);
                    parameters.Add(parameterPassword);
                    parameters.Add(parameterIsActive);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddRange(parameters.ToArray());
                    var reader = command.ExecuteReader();
                    
                    if (!reader.HasRows) return null;

                    while (reader.Read())
                    {
                        var id = reader.GetInt32("Id");
                        product.Id = id;

                        break;
                    }

                    return product;
                }
            }
            
            if(_connection == null)
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                    return Execute(connection);
                }
            }
            else
            {
                return Execute(_connection);
            }
        }
        
        bool disposed = false;
        
        public void Dispose()
        { 
            Dispose(true);
            GC.SuppressFinalize(this);           
        }
   
        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return; 
      
            if (disposing) {
                // Free any other managed objects here.
            }
      
            disposed = true;
        }

        ~MariaDbRepository()
        {
            Dispose(false);
        }
        
        private struct StoredProcedures
        {
            public const string SpUserCreateUser = "sp_User_CreateUser";
            public const string SpUserGetUsers = "sp_User_GetUsers";
            public const string SpUserUpdateUser = "sp_User_UpdateUser";
            public const string SpProductCreateProduct = "sp_Product_CreateProduct";
            public const string SpProductGetProducts = "sp_Product_GetProducts";
        }
    }
}