using Domain;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace MySqlPersistance
{
    public class MySqlActionItemRepositoy : IActionItemRepository
    {
        private readonly MySqlDbContext context;
        private readonly IUserRepository userRepository;

        public MySqlActionItemRepositoy(MySqlPersistanceContext persitanceContext)
        {
            this.context = persitanceContext.GetContext();
            this.userRepository = persitanceContext.GetUserRepository();
        }

        public void Add(ActionItem actionItem)
        {
            using var cmd = context.Connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO `ActionItems` (`Id`, `Content`, `Completed`, `CreatedAt`, `UserId`) VALUES (@Id, @Content, @Completed, @CreatedAt, @UserId);";
            actionItem.BindAcionItemParams(cmd);
            var result = cmd.ExecuteNonQuery();
        }

        public void Delete(Guid id)
        {
            using var cmd = context.Connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM `ActionItems` WHERE `Id` = @Id;";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Id",
                DbType = DbType.String,
                Value = id.ToString()
            });
            cmd.ExecuteNonQuery();
        }

        public void Edit(ActionItem actionItem)
        {
            using var cmd = context.Connection.CreateCommand();
            cmd.CommandText = @"UPDATE `ActionItems` SET `Content` = @Content, `Completed` = @Completed WHERE `Id` = @Id";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Id",
                DbType = DbType.String,
                Value = actionItem.Id.ToString()
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Content",
                DbType = DbType.String,
                Value = actionItem.Content
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Completed",
                DbType = DbType.Byte,
                Value = actionItem.Completed
            });
            cmd.ExecuteNonQuery();
        }

        public ActionItem Get(Guid id)
        {
            using var cmd = context.Connection.CreateCommand();
            cmd.CommandText = @"SELECT `Id`, `Content`, `Completed`, `CreatedAt`, `UserId` FROM `ActionItems`
                                WHERE `Id` = @Id;";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Id",
                DbType = DbType.String,
                Value = id.ToString()
            });

            try
            {
                ActionItem item;
                Guid userId;
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read() == false)
                    {
                        throw new Exception();
                    }
                    item = ReadItem(reader);
                    userId = reader.GetGuid(4);
                }

                return ReadUserForItem(item, userId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public IEnumerable<ActionItem> GetAll(string userId)
        {
            using var cmd = context.Connection.CreateCommand();
            cmd.CommandText = @"SELECT `Id`, `Content`, `Completed`, `CreatedAt`, `UserId` FROM `ActionItems`
                                WHERE `UserId` = @UserId;";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@UserId",
                DbType = DbType.String,
                Value = userId
            });

            try
            {
                var touple_items = new List<Tuple<ActionItem, Guid>>();
                var items = new List<ActionItem>();

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        touple_items.Add(new Tuple<ActionItem, Guid>(ReadItem(reader), reader.GetGuid(4)));
                    }
                }
                foreach(var item in touple_items)
                {
                    items.Add(ReadUserForItem(item.Item1, item.Item2));
                }
                return items;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public void Save()
        {
            // this method has no purpose in here actually
            // we don't have a queue mechanism in here, but I can make one if i think about it
            // man, this starts to sound so good
            // I am starting to enjoy this database thing
        }

        private ActionItem ReadItem(MySqlDataReader reader)
        {
            return new ActionItem
            {
                Id = reader.GetGuid(0),
                Content = reader.GetString(1),
                Completed = Convert.ToBoolean(reader.GetByte(2)),
                CreatedAt = reader.GetDateTime(3)
            };
        }

        private ActionItem ReadUserForItem(ActionItem item, Guid id)
        {
            item.User = userRepository.GetUser(id);
            return item;
        }
    }
}
