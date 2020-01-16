using System;

namespace Domain
{
    public interface IUserRepository
    {
        User RegisterUser(string username, string password);
        User GetUser(string username);
        User GetUser(Guid id);
    }
}
