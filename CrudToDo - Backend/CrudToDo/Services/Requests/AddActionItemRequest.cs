using System;

namespace CrudToDo.Services.Requests
{
    public class AddActionItemRequest
    {
        public string Content { get; set; }
        public bool Completed { get; set; }
    }
}
