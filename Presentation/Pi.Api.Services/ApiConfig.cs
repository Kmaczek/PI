using Microsoft.Extensions.Configuration;
using Pi.Api.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pi.Api.Services
{
    public class ApiConfig : IApiConfig
    {
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
    }
}
