using Pi.Api.EF;
using Pi.Api.EF.Models.Auth;
using Pi.Api.EF.Repository.Interfaces;
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

        public AppUser GetUser(string username)
        {
            var user = PiContext.ApplicationUsers.FirstOrDefault(x => x.Username == username);

            return user;
        }

        public AppUser InsertUser(AppUser user)
        {
            PiContext.ApplicationUsers.Add(user);
            PiContext.SaveChanges();

            return user;
        }
    }
}
