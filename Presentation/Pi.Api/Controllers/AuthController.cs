using Data.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Pi.Api.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAppUserRepository userRepository;

        public AuthController(IAppUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public IActionResult Login([FromBody]LoginUser credentials)
        {
            var user = userRepository.GetUser(credentials.Username);
            return Ok(user);
        }
    }

    public class LoginUser
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
