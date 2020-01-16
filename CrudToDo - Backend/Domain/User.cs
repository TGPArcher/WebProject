using System;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class User
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }

        public static User Create(string username, string password)
        {
            return new User()
            {
                Id = Guid.NewGuid(),
                Username = username,
                Password = password
            };
        }
    }
}
