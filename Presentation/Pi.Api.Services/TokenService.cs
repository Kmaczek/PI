using Core.Common;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Pi.Api.EF.Models.Auth;
using Pi.Api.Services.Interfaces;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Pi.Api.Services
{
    public class TokenService : ITokenService
    {
        private readonly ILogger log;
        private readonly IApiConfig apiConfig;

        public TokenService(
            ILogger log,
            IApiConfig apiConfig)
        {
            this.log = log;
            this.apiConfig = apiConfig;

            if(ValidateConfig() == false)
            {
                throw new Exception("Config variables are not set properly. Can't create token.");
            }
        }

        public async Task<string> CreateJwtAsync(
            AppUser user,
            int daysValid)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var claims = await CreateClaimsIdentities(user);
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(apiConfig.PrivateKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = tokenHandler.CreateJwtSecurityToken(
                issuer: apiConfig.Issuer,
                audience: apiConfig.Issuer,
                subject: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddDays(daysValid),
                signingCredentials: creds); 

            return tokenHandler.WriteToken(token);
        }

        private Task<ClaimsIdentity> CreateClaimsIdentities(AppUser user)
        {
            ClaimsIdentity claimsIdentity = new ClaimsIdentity();
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Email, user.Email));
            claimsIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id + ""));
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, user.Username));
            claimsIdentity.AddClaim(new Claim(ClaimTypes.UserData, JsonConvert.SerializeObject(user)));

            //foreach (var role in roles)
            //{ claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, "admin")); }

            claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, "admin"));

            return Task.FromResult(claimsIdentity);
        }

        private bool ValidateConfig()
        {
            var result = true;

            if (string.IsNullOrEmpty(apiConfig.PrivateKey))
            {
                log.Error("Config variable [PrivateKey] is empty.");
                result = false;
            }

            if (string.IsNullOrEmpty(apiConfig.Issuer))
            {
                log.Error("Config variable [Issuer] is empty.");
                result = false;
            }

            return result;
        }
    }
}
