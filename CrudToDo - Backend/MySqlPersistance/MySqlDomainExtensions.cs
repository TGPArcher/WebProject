using Domain;
using MySql.Data.MySqlClient;
using System.Data;

namespace MySqlPersistance
{
    internal static class MySqlDomainExtensions
    {
        public static void BindUserParams(this User user, MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Id",
                DbType = DbType.String,
                Value = user.Id.ToString(),
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Username",
                DbType = DbType.String,
                Value = user.Username
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Password",
                DbType = DbType.String,
                Value = user.Password
            });
        }

        public static void BindAcionItemParams(this ActionItem item, MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Id",
                DbType = DbType.String,
                Value = item.Id.ToString()
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Content",
                DbType = DbType.String,
                Value = item.Content
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Completed",
                DbType = DbType.Byte,
                Value = item.Completed
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@CreatedAt",
                DbType = DbType.Date,
                Value = item.CreatedAt
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@UserId",
                DbType = DbType.String,
                Value = item.User.Id.ToString()
            });
        }
    }
}
