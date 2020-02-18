using System;
using System.Collections.Generic;
using System.Text;

namespace Pi.Api.Services.Interfaces
{
    public interface IApiConfig
    {
        string PrivateKey { get; }
        string Issuer { get; }
    }
}
