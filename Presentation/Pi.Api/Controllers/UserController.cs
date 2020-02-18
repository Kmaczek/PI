using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pi.Api.EF.Repository.Interfaces;

namespace Pi.Api.Controllers
{
    [ApiController]
    [Route("user")]
    //[Authorize]
    public class UserController : ControllerBase
    {
        private readonly IAppUserRepository userRepository;

        public UserController(IAppUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        [HttpGet]
        [Authorize(Roles = "View")]
        public IActionResult GetUser(string username)
        {
            var user = userRepository.GetUser(username);

            return Ok(user);
        }
    }
}
