using Domain;
using System;

namespace CrudToDo.Services
{
    public class ActionItemInfo
    {
        public string Id { get; set; }
        public string Content { get; set; }
        public bool Completed { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UserId { get; set; }

        public static ActionItemInfo MapFrom(ActionItem item)
        {
            return new ActionItemInfo()
            {
                Id = item.Id.ToString(),
                Content = item.Content,
                Completed = item.Completed,
                CreatedAt = item.CreatedAt,
                UserId = item.User.Id.ToString()
            };
        }
    }
}
