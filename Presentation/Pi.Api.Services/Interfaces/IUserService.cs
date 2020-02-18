using System;
using System.Collections.Generic;
using System.Text;

namespace Pi.Api.Services.Interfaces
{
    public interface IUserService
    {
        void CreateUser(string username, string password);
        string LoginUser(string username, string password);
    }
}
