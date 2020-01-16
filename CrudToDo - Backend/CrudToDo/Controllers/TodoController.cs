using CrudToDo.Services;
using CrudToDo.Services.Requests;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;

namespace CrudToDo.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [Authorize]
    public class TodoController : Controller
    {
        private readonly ActionItemService itemService;
        private readonly UserService userService;

        public TodoController(ActionItemService itemService, UserService userService)
        {
            this.itemService = itemService;
            this.userService = userService;
        }

        private User AppUser()
        {
            var username = HttpContext.User.Identity.Name;
            return userService.GetUserByUsername(username);
        }

        [HttpGet]
        [Route("")]
        public IActionResult GetAll()
        {
            try
            {
                var user = AppUser();
                var items = itemService.GetAll(user.Id.ToString());
                if(items == null)
                {
                    return NotFound();
                }
                return Ok(items);
            }
            catch
            {
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        [Route("{id:guid}")]
        public IActionResult Get(string id)
        {
            try
            {
                var user = AppUser();
                var item = itemService.Get(id);
                if(item == null)
                {
                    return NotFound();
                }
                if(Guid.Parse(item.UserId) != user.Id)
                {
                    return BadRequest();
                }
                return Ok(item);
            }
            catch
            {
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        public IActionResult Add([FromBody]AddActionItemRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Content))
                {
                    return BadRequest();
                }
                var item = ActionItem.Create(request.Content, request.Completed, DateTime.Now);
                var user = AppUser();
                item.User = user;
                itemService.Add(item);
                return Ok(item);
            }
            catch
            {
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPut]
        [Route("{id:guid}")]
        public IActionResult Update(string id, [FromBody]AddActionItemRequest request)
        {
            try
            {
                if(string.IsNullOrEmpty(id) || string.IsNullOrEmpty(request.Content))
                {
                    return BadRequest();
                }
                var user = AppUser();
                var item = itemService.Get(id);
                if(item == null || Guid.Parse(item.UserId) != user.Id)
                {
                    return BadRequest();
                }
                item.Content = request.Content;
                item.Completed = request.Completed;
                itemService.Edit(item);
                return Ok(item);
            }
            catch
            {
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public IActionResult Delete(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    return NotFound();
                }
                var user = AppUser();
                var item = itemService.Get(id);
                if(user.Id != Guid.Parse(item.UserId))
                {
                    return BadRequest();
                }
                itemService.Delete(id);
                return Ok();
            }
            catch
            {
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}