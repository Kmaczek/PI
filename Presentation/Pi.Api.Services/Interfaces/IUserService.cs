using Pi.Api.EF.Models.Auth;
using Pi.APi.Models;
using Pi.APi.Models.User;
using System.Threading.Tasks;

namespace Pi.Api.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserVm> GetUser(int userId);
        Task<AppUserDm> CreateUser(string username, string email, string password);
        Task<LoginResult> LoginUser(string email, string password);
    }
}
