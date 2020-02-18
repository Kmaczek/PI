using Pi.Api.EF.Models.Auth;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pi.Api.Services.Interfaces
{
    public interface ITokenService
    {
        Task<string> CreateJwtAsync(AppUserDm user, IEnumerable<string> roles, int daysValid);
    }
}
