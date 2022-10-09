using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pi.Api.Services.Interfaces;

namespace Pi.Api.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase //ApiController
    {
        private readonly IUserService userService;

        public AuthController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public IActionResult Login([FromBody]LoginUser credentials)
        {
            var token = userService.LoginUser(credentials.Username, credentials.Password);

            if (!string.IsNullOrEmpty(token))
            {
                return Ok(new { token = token });
            }

            return Unauthorized("Incorrect user or password.");
        }

        [HttpPost("create")]
        //[Authorize(Roles = "admin")]
        public IActionResult CreateUser([FromBody]LoginUser credentials)
        {
            userService.CreateUser(credentials.Username, credentials.Password);

            return Ok();
        }
        
    }

    public class LoginUser
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
