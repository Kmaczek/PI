using Pi.Api.EF.Models.Auth;
using System.Threading.Tasks;

namespace Pi.Api.Services.Interfaces
{
    public interface ITokenService
    {
        Task<string> CreateJwtAsync(AppUser user, int daysValid);
    }
}
