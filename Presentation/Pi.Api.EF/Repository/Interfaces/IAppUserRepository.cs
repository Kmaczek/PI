using Pi.Api.EF.Models.Auth;

namespace Pi.Api.EF.Repository.Interfaces
{
    public interface IAppUserRepository
    {
        AppUser GetUser(string username);
        AppUser InsertUser(AppUser user);
    }
}
