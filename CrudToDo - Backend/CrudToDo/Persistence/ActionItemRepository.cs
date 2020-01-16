using Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CrudToDo.Persistence
{
    public class ActionItemRepository : IActionItemRepository
    {
        CoreDbContext context;

        public ActionItemRepository(CoreDbContext context)
        {
            this.context = context;
        }

        public void Add(ActionItem actionItem)
        {
            context.ActionItems.Add(actionItem);
            context.SaveChanges();
        }

        public void Edit(ActionItem actionItem)
        {
            context.ActionItems.Update(actionItem);
            context.SaveChanges();
        }

        public void Delete(Guid id)
        {
            var entity = Get(id);
            context.ActionItems.Remove(entity);
            context.SaveChanges();
        }

        public ActionItem Get(Guid id)
        {
            return context.ActionItems.Find(id);
        }

        public IEnumerable<ActionItem> GetAll(string userId)
        {
            var userGuid = Guid.Parse(userId);
            return context.ActionItems.Where(i => i.User.Id == userGuid);
        }

        public void Save()
        {
            context.SaveChanges();
        }
    }
}
