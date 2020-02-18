using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pi.Api.EF.Models.Auth;
using Pi.Api.EF.Repository.Interfaces;
using Pi.Api.Services.Interfaces;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

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
        public IActionResult Login([FromBody]LoginUser credentials)
        {
            var token = userService.LoginUser(credentials.Username, credentials.Password);

            if (!string.IsNullOrEmpty(token))
            {
                return Ok(token);
            }

            return Unauthorized("Incorrect user or password.");
        }

        [HttpPost("create")]
        [Authorize(Roles = "Admin")]
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
