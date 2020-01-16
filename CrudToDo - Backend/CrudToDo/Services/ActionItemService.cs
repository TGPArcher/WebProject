using Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CrudToDo.Services
{
    public class ActionItemService
    {
        IActionItemRepository itemRepository;

        public ActionItemService(IPersistanceContext persistenceContext)
        {
            itemRepository = persistenceContext.GetActionItemRepository();
        }

        public IEnumerable<ActionItemInfo> GetAll(string userId)
        {
            if(userId == null)
            {
                return null;
            }
            var items = itemRepository.GetAll(userId);
            var infos = items.Select(i => ActionItemInfo.MapFrom(i)).ToList();
            return infos;
        }

        public ActionItemInfo Get(string id)
        {
            if (string.IsNullOrEmpty(id))
                return null;

            var item = itemRepository.Get(Guid.Parse(id));
            var info = ActionItemInfo.MapFrom(item);
            return info;
        }

        public void Add(ActionItem item)
        {
            if (item == null)
                return;

            itemRepository.Add(item);
            itemRepository.Save();
        }

        public void Edit(ActionItemInfo item)
        {
            if (item == null)
                return;
            var dbItem = itemRepository.Get(Guid.Parse(item.Id));
            dbItem.Content = item.Content;
            dbItem.Completed = item.Completed;
            itemRepository.Edit(dbItem);
            itemRepository.Save();
        }

        public bool Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
                return false;

            var item = Get(id);
            if (item == null)
                return false;

            itemRepository.Delete(Guid.Parse(item.Id));
            itemRepository.Save();
            return true;
        }
    }
}
