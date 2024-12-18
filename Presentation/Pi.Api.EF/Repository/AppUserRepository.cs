using Microsoft.EntityFrameworkCore;
using Pi.Api.EF.Models.Auth;
using Pi.Api.EF.Repository.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pi.Api.EF.Repository
{
    public class AppUserRepository : IAppUserRepository
    {
        ApiContext PiContext;

        public AppUserRepository(ApiContext dbContext)
        {
            PiContext = dbContext;
        }

        public Task<AppUserDm> GetUser(string email)
        {
            return PiContext.ApplicationUsers.FirstOrDefaultAsync(x => x.Email == email);
        }

        public Task<AppUserDm> GetUser(int userId)
        {
            return PiContext.ApplicationUsers.FirstOrDefaultAsync(x => x.Id == userId);
        }

        public Task<List<RoleDm>> GetUserRoles(int userId)
        {
            return PiContext.ApplicationUsers
                .Where(x => x.Id == userId)
                .Include(x => x.UserRoles)
                .ThenInclude(userRoles => userRoles.Role)
                .SelectMany(x => x.UserRoles.Select(y => y.Role)).ToListAsync();
        }

        public async Task<AppUserDm> InsertUser(AppUserDm user)
        {
            PiContext.ApplicationUsers.Add(user);
            await PiContext.SaveChangesAsync();

            return user;
        }
    }
}
