using CrudToDo.Persistence;
using Domain;
using System;

namespace CrudToDo.Services
{
    public class UserService
    {
        IUserRepository repository = null;

        public UserService(IPersistanceContext context)
        {
            this.repository = context.GetUserRepository();
        }

        public User Register(string username, string password)
        {
            if(string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                return null;
            }
            return repository.RegisterUser(username, password);
        }

        public User GetUser(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return null;
            }
            return repository.GetUser(Guid.Parse(id));
        }

        public User GetUserByUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                return null;
            }
            return repository.GetUser(username);
        }
    }
}
