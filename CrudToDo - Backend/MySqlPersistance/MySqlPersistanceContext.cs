using Domain;
using System;

namespace MySqlPersistance
{
    public class MySqlPersistanceContext : IPersistanceContext
    {
        private MySqlDbContext context;
        private IUserRepository userRepository;
        private IActionItemRepository actionItemRepository;

        public MySqlPersistanceContext(IServiceProvider serviceProvider)
        {
            if(context == null)
            {
                context = serviceProvider.GetService(typeof(MySqlDbContext)) as MySqlDbContext;
            }

            context.EnsureCreated();

            userRepository = new MySqlUserRepository(this);
            actionItemRepository = new MySqlActionItemRepositoy(this);
        }

        public MySqlDbContext GetContext()
        {
            return context;
        }

        public IActionItemRepository GetActionItemRepository()
        {
            if (actionItemRepository == null)
            {
                actionItemRepository = new MySqlActionItemRepositoy(this);
            }
            return actionItemRepository;
        }

        public IUserRepository GetUserRepository()
        {
            if (userRepository == null)
            {
                userRepository = new MySqlUserRepository(this);
            }
            return userRepository;
        }
    }
}
