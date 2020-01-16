using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace CrudToDo.Persistence
{
    public class CorePersistenceContext : IPersistanceContext
    {
        CoreDbContext context;
        ActionItemRepository actionItemRepository;
        UserRepository userRepository;

        public CorePersistenceContext(IServiceProvider serviceProvider)
        {
            if(context == null)
            {
                context = serviceProvider.GetService<CoreDbContext>();
            }

            context?.Database.Migrate();

            actionItemRepository = new ActionItemRepository(context);
            userRepository = new UserRepository(context);
        }

        public IActionItemRepository GetActionItemRepository()
        {
            if(actionItemRepository == null)
            {
                actionItemRepository = new ActionItemRepository(context);
            }
            return actionItemRepository;
        }

        public IUserRepository GetUserRepository()
        {
            if(userRepository == null)
            {
                userRepository = new UserRepository(context);
            }
            return userRepository;
        }
    }
}
