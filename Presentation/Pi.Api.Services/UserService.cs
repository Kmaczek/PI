using Pi.Api.EF.Models.Auth;
using Pi.Api.EF.Repository.Interfaces;
using Pi.Api.Services.Interfaces;
using Pi.APi.Models;
using Pi.APi.Models.User;
using System;
using System.Linq;
using System.Threading.Tasks;

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

        public Task<AppUserDm> CreateUser(string username, string email, string password)
        {
            var user = new AppUserDm()
            {
                ActiveFromUtc = DateTime.UtcNow,
                ActiveToUtc = DateTime.UtcNow.AddYears(100),
                CreatedDateUtc = DateTime.UtcNow,
                Email = email,
                DisplayName = username,
            };
            var hashedPassword = passwordHashing.HashPassword(password);

            user.Password = hashedPassword.Password;
            user.Salt = hashedPassword.Salt;

            return userRepository.InsertUser(user);
        }

        public async Task<LoginResult> LoginUser(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                return LoginResult.Failed(LoginErrorType.InvalidInput, "Email and password are required");
            }

            var user = await userRepository.GetUser(email);
            if (user == null)
            {
                return LoginResult.Failed(LoginErrorType.UserNotFound, "User not found");
            }

            if (!passwordHashing.ComparePasswords(password, user.Salt, user.Password))
            {
                return LoginResult.Failed(LoginErrorType.InvalidPassword, "Invalid password");
            }

            var roles = (await userRepository.GetUserRoles(user.Id)).Select(x => x.Name);
            var daysValid = TimeSpan.FromHours(apiConfig.TokenExpirationTime).Days;
            var token = await tokenService.CreateJwtAsync(user, roles, daysValid);

            return LoginResult.Successful(token);
        }

        public async Task<UserVm> GetUser(int userId)
        {
            var user = (await userRepository.GetUser(userId)) ?? throw new InvalidOperationException("User not found");

            var roles = await userRepository.GetUserRoles(user.Id);

            var userVm = UserVm.FromDm(user, roles);

            return userVm;
        }
    }
}
