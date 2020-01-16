using Domain;
using System;
using System.Linq;

namespace CrudToDo.Persistence
{
    public class UserRepository : IUserRepository
    {
        private CoreDbContext context;
        public UserRepository(CoreDbContext context)
        {
            this.context = context;
        }

        public User GetUser(string username)
        {
            return context.Users.Where(u => u.Username == username).FirstOrDefault();
        }

        public User GetUser(Guid id)
        {
            return context.Users.Find(id);
        }

        public User RegisterUser(string username, string password)
        {
            var user = User.Create(username, password);
            context.Users.Add(user);
            context.SaveChanges();
            return user;
        }
    }
}
