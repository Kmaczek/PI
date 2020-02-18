using Microsoft.Extensions.Configuration;
using Pi.Api.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pi.Api.Services
{
    public class ApiConfig : IApiConfig
    {
        private const int DefaultTokenExpirationTime = 8;
        private readonly IConfigurationRoot configuration;

        public ApiConfig(IConfigurationRoot configuration)
        {
            this.configuration = configuration;
        }

        public string PrivateKey
        {
            get
            {
                var privateKey = configuration.GetSection("PI_TokenPrivateKey").Value;

                return privateKey;
            }
        }

        public string Issuer
        {
            get
            {
                var privateKey = configuration["Jwt:Issuer"];

                return privateKey;
            }
        }

        public int TokenExpirationTime
        {
            get
            {
                int result;
                var parsingResult = Int32.TryParse(configuration["Jwt:HoursValid"], out result);

                return parsingResult ? result : DefaultTokenExpirationTime;
            }
        }
    }
}
