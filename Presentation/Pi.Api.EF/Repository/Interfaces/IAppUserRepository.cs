using Pi.Api.EF.Models.Auth;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pi.Api.EF.Repository.Interfaces
{
    public interface IAppUserRepository
    {
        Task<AppUserDm> GetUser(int userId);
        Task<AppUserDm> GetUser(string email);
        Task<List<RoleDm>> GetUserRoles(int userId);
        Task<AppUserDm> InsertUser(AppUserDm user);
    }
}
