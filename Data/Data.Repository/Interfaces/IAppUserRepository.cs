using Data.EF.Models.Auth;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Repository.Interfaces
{
    public interface IAppUserRepository
    {
        AppUser GetUser(string username);
    }
}
