using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pi.Api.Services.Interfaces;
using Pi.APi.Models.User;
using System.Threading.Tasks;

namespace Pi.Api.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService userService;

        public AuthController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginUser credentials)
        {
            var result = await userService.LoginUser(credentials.Email, credentials.Password);

            if (result.Success)
            {
                return Ok(new { token = result.Token });
            }

            return result.ErrorType switch
            {
                LoginErrorType.UserNotFound => NotFound(new { message = "User not found" }),
                LoginErrorType.InvalidPassword => Unauthorized(new { message = "Invalid credentials" }),
                LoginErrorType.AccountLocked => StatusCode(
                    StatusCodes.Status403Forbidden,
                    new { message = "Account is locked" }),
                LoginErrorType.InvalidInput => BadRequest(new { message = result.Error }),
                _ => StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new { message = "An unexpected error occurred" })
            };
        }

        [HttpPost("create")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUser credentials)
        {
            var user = await userService.CreateUser(credentials.Username, credentials.Email, credentials.Password);

            return user != null ? Created("", user) : BadRequest();
        }

    }

    public class LoginUser
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class CreateUser
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
