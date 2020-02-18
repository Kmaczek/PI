using Pi.Api.EF.Models.Auth;
using System.Collections.Generic;

namespace Pi.Api.EF.Repository.Interfaces
{
    public interface IAppUserRepository
    {
        AppUserDm GetUser(string username);
        IEnumerable<RoleDm> GetUserRoles(string username);

        AppUserDm InsertUser(AppUserDm user);
    }
}
