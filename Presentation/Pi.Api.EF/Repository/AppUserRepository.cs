using Microsoft.EntityFrameworkCore;
using Pi.Api.EF;
using Pi.Api.EF.Models.Auth;
using Pi.Api.EF.Repository.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Data.Repository
{
    public class AppUserRepository : IAppUserRepository
    {
        ApiContext PiContext;

        public AppUserRepository(ApiContext dbContext)
        {
            PiContext = dbContext;
        }

        public AppUserDm GetUser(string username)
        {
            var user = PiContext.ApplicationUsers.FirstOrDefault(x => x.Username == username);

            return user;
        }

        public IEnumerable<RoleDm> GetUserRoles(string username)
        {
            var userRoles = PiContext.ApplicationUsers
                .Where(x => x.Username == username)
                .Include(x => x.UserRoles)
                .ThenInclude(userRoles => userRoles.Role)
                .SelectMany(x => x.UserRoles.Select(y => y.Role)).ToList();

            return userRoles;
        }

        public AppUserDm InsertUser(AppUserDm user)
        {
            PiContext.ApplicationUsers.Add(user);
            PiContext.SaveChanges();

            return user;
        }
    }
}
