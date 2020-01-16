using Domain;
using MySql.Data.MySqlClient;
using System;
using System.Data;

namespace MySqlPersistance
{
    public class MySqlUserRepository : IUserRepository
    {
        private readonly MySqlDbContext context;

        public MySqlUserRepository(MySqlPersistanceContext persitanceContext)
        {
            this.context = persitanceContext.GetContext();
        }

        public User GetUser(string username)
        {
            using var cmd = context.Connection.CreateCommand();
            cmd.CommandText = @"SELECT `Id`, `Username`, `Password` FROM `Users` WHERE `Username` = @Username;";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Username",
                DbType = DbType.String,
                Value = username
            });
            
            using var reader = cmd.ExecuteReader();
            try
            {
                if (reader.Read() == false)
                {
                    throw new Exception();
                }

                var user = new User
                {
                    Id = reader.GetGuid(0),
                    Username = reader.GetString(1),
                    Password = reader.GetString(2)
                };
                return user;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public User GetUser(Guid id)
        {
            using var cmd = context.Connection.CreateCommand();
            cmd.CommandText = @"SELECT `Id`, `Username`, `Password` FROM `Users` WHERE `Id` = @Id;";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Id",
                DbType = DbType.String,
                Value = id
            });

            using var reader = cmd.ExecuteReader();
            try
            {

                if(reader.Read() == false)
                {
                    throw new Exception();
                }

                var user = new User
                {
                    Id = reader.GetGuid(0),
                    Username = reader.GetString(1),
                    Password = reader.GetString(2)
                };
                return user;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public User RegisterUser(string username, string password)
        {
            var user = User.Create(username, password);

            using var cmd = context.Connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO `Users` (`Id`, `Username`, `Password`) VALUES (@Id, @Username, @Password);";
            MySqlDomainExtensions.BindUserParams(user, cmd);
            var result = cmd.ExecuteNonQuery();

            if(result == 0)
            {
                return null;
            }
            return user;
        }
    }
}
