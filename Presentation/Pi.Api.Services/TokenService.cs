using Core.Common.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Pi.Api.EF.Models.Auth;
using Pi.Api.Services.Interfaces;
using System;
using System.Collections.Generic;
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
            AppUserDm user,
            IEnumerable<string> roles,
            int daysValid)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var claims = await CreateClaimsIdentities(user, roles);
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

        private Task<ClaimsIdentity> CreateClaimsIdentities(AppUserDm user, IEnumerable<string> roles)
        {
            ClaimsIdentity claimsIdentity = new ClaimsIdentity();
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Email, user.Email));
            claimsIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, user.Username));
            claimsIdentity.AddClaim(new Claim(ClaimTypes.GivenName, user.DisplayName));
            claimsIdentity.AddClaim(new Claim(ClaimTypes.UserData, JsonConvert.SerializeObject(new { DisplayName = user.DisplayName})));

            foreach (var role in roles)
            { claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, role)); }

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
