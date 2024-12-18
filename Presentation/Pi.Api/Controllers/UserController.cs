using Microsoft.AspNetCore.Mvc;
using Pi.Api.Services.Interfaces;
using System.Threading.Tasks;

namespace Pi.Api.Controllers
{
    [ApiController]
    [Route("user")]
    public class UserController(IUserService userService) : SecureController
    {
        private readonly IUserService _userService = userService;

        [HttpGet]
        public async Task<IActionResult> GetUser()
        {
            var user = await _userService.GetUser(UserId);

            return Ok(user);
        }
    }
}
