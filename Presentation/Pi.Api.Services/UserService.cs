using Pi.Api.EF.Models.Auth;
using Pi.Api.EF.Repository.Interfaces;
using Pi.Api.Services.Interfaces;
using System;
using System.Linq;

namespace Pi.Api.Services
{
    public class UserService : IUserService
    {
        private readonly IAppUserRepository userRepository;
        private readonly IPasswordHashing passwordHashing;
        private readonly ITokenService tokenService;
        private readonly IApiConfig apiConfig;

        public UserService(
            IAppUserRepository userRepository,
            IPasswordHashing passwordHashing,
            ITokenService tokenService,
            IApiConfig apiConfig)
        {
            this.userRepository = userRepository;
            this.passwordHashing = passwordHashing;
            this.tokenService = tokenService;
            this.apiConfig = apiConfig;
        }

        public void CreateUser(string username, string password)
        {
            var user = new AppUserDm()
            {
                ActiveFrom = DateTime.Now,
                ActiveTo = DateTime.Now.AddYears(100),
                CreatedDate = DateTime.Now,
                DisplayName = username,
                Username = username
            };
            var hashedPassword = passwordHashing.HashPassword(password);

            user.Password = hashedPassword.Password;
            user.Salt = hashedPassword.Salt;

            userRepository.InsertUser(user);
        }

        public string LoginUser(string username, string password)
        {
            var user = userRepository.GetUser(username);
            var roles = userRepository.GetUserRoles(username).Select(x => x.Name);

            if (passwordHashing.ComparePasswords(password, user.Salt, user.Password))
            {
                var daysValid = TimeSpan.FromHours(apiConfig.TokenExpirationTime).Days;
                var token = tokenService.CreateJwtAsync(user, roles, daysValid);

                return token.Result;
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
