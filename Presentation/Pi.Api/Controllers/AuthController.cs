using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Pi.Api.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        public AuthController()
        {

        }

        [HttpPost("login")]
        [AllowAnonymous]
        public IActionResult Login([FromBody]LoginUser credentials)
        {
            return Ok();
        }
    }

    public class LoginUser
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
