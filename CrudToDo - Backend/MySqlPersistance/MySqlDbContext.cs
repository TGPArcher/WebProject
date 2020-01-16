using MySql.Data.MySqlClient;
using System;

namespace MySqlPersistance
{
    public class MySqlDbContext : IDisposable
    {
        public MySqlConnection Connection { get; }

        public MySqlDbContext(string connectionString)
        {
            Connection = new MySqlConnection(connectionString);
            Connection.Open();
        }

        public void Dispose()
        {
            Connection.Close();
            Connection.Dispose();
        }

        public void EnsureCreated()
        {
            // Create database schema
            using(var cmd = Connection.CreateCommand())
            {
                cmd.CommandText = @"CREATE SCHEMA IF NOT EXISTS todo;";
                cmd.ExecuteNonQuery();
            }

            // Use database schema
            using(var cmd = Connection.CreateCommand())
            {
                cmd.CommandText = @"USE todo;";
                cmd.ExecuteNonQuery();
            }

            // Create Users table
            using(var cmd = Connection.CreateCommand())
            {
                cmd.CommandText = @"
                                    CREATE TABLE IF NOT EXISTS Users (
                                        `Id`       CHAR(36) NOT NULL,
                                        `Username` VARCHAR(50)    NOT NULL,
                                        `Password` LONGTEXT    NOT NULL,
                                        CONSTRAINT `PK_Users` PRIMARY KEY (`Id` ASC),
                                        CONSTRAINT `UNIQUE_Username` UNIQUE (Username)
                                    );";
                cmd.ExecuteNonQuery();
            }

            // Create ActionItem table
            using (var cmd = Connection.CreateCommand())
            {
                cmd.CommandText = @"
                                    CREATE TABLE IF NOT EXISTS ActionItems (
                                    `Id`        CHAR(36) NOT NULL,
                                    `Content`   LONGTEXT    NOT NULL,
                                    `Completed` TINYINT              NOT NULL,
                                    `CreatedAt` DATETIME (6)    NOT NULL,
                                    `UserId`    CHAR(36) DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
                                    CONSTRAINT `PK_ActionItems` PRIMARY KEY (`Id` ASC),
                                    CONSTRAINT `FK_ActionItems_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES Users (`Id`) ON DELETE CASCADE
                                    );";
                cmd.ExecuteNonQuery();
            }
        }
    }
}
