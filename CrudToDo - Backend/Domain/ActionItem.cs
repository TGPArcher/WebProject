using System;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class ActionItem
    {
        public Guid Id { get; set; }
        [Required]
        public string Content { get; set; }
        public bool Completed { get; set; }
        public DateTime CreatedAt { get; set; }
        [Required]
        public virtual User User { get; set; }

        public static ActionItem Create(string content, bool completed, DateTime createdAt)
        {
            return new ActionItem
            {
                Id = Guid.NewGuid(),
                Content = content,
                Completed = completed,
                CreatedAt = createdAt,
            };
        }
    }
}
