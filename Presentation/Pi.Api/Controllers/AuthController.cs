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
        private readonly IAppUserRepository userRepository;
        private readonly ITokenService tokenService;

        public AuthController(
            IAppUserRepository userRepository, 
            ITokenService tokenService)
        {
            this.userRepository = userRepository;
            this.tokenService = tokenService;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public IActionResult Login([FromBody]LoginUser credentials)
        {
            var user = userRepository.GetUser(credentials.Username);
            var passwordHash = Hash(credentials.Password, user.Salt);

            if (Enumerable.SequenceEqual(passwordHash, user.Password))
            {
                var daysValid = 7;
                var token = tokenService.CreateJwtAsync(user, daysValid);

                return Ok(token.Result);
            }

            return Ok("Incorrect user or password.");
        }

        [HttpPost("create")]
        [Authorize(Roles = "admin")]
        public IActionResult CreateUser([FromBody]LoginUser credentials)
        {
            var user = new AppUser()
            {
                ActiveFrom = DateTime.Now,
                ActiveTo = DateTime.Now.AddYears(100),
                CreatedDate = DateTime.Now,
                DisplayName = "Damian Kmak",
                Username = credentials.Username
            };
            var salt = GetSalt(50);
            var passwordHash = Hash(credentials.Password, salt);
            user.Password = passwordHash;
            user.Salt = salt;

            userRepository.InsertUser(user);

            return Ok(user);
        }
        

        //Hash Password
        public static byte[] Hash(string value, byte[] salt)
        {
            return Hash(Encoding.UTF8.GetBytes(value), salt);
        }

        public static byte[] Hash(byte[] value, byte[] salt)
        {
            byte[] saltedValue = value.Concat(salt).ToArray();

            return new SHA256Managed().ComputeHash(saltedValue);
        }

        private static byte[] GetSalt(int maximumSaltLength)
        {
            var salt = new byte[maximumSaltLength];
            using (var random = new RNGCryptoServiceProvider())
            {
                random.GetNonZeroBytes(salt);
            }

            return salt;
        }
        /////////
    }

    public class LoginUser
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
