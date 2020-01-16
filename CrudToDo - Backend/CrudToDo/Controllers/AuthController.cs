using CrudToDo.Services;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CrudToDo.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserService userService;

        public AuthController(UserService userService)
        {
            this.userService = userService;
        }

        [Authorize]
        [Route("login/{username}")]
        [HttpGet]
        public IActionResult Login(string username)
        {
            var user = userService.GetUserByUsername(username);
            return Ok(user.Id);
        }

        [Route("register")]
        [HttpPost]
        public IActionResult Register([FromBody]User user)
        {
            if(user != null)
            {
                var registeredUser = userService.Register(user.Username, user.Password);
                if(registeredUser != null)
                {
                    return Ok(registeredUser);
                }
            }
            return BadRequest();
        }
    }
}