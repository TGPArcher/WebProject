using System;
using System.Collections.Generic;

namespace Domain
{
    public interface IActionItemRepository
    {
        void Add(ActionItem actionItem);
        void Edit(ActionItem actionItem);
        void Delete(Guid id);
        ActionItem Get(Guid id);
        IEnumerable<ActionItem> GetAll(string userId);
        void Save();
    }
}
