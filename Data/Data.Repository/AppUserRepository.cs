using Data.EF.Models;
using Data.EF.Models.Auth;
using Data.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Repository
{
    public class AppUserRepository : IAppUserRepository
    {
        PiContext PiContext;

        public AppUserRepository(PiContext dbContext)
        {
            PiContext = dbContext;
        }

        public AppUser GetUser(string username)
        {
            var user = PiContext.ApplicationUsers.FirstOrDefault(x => x.Username == username);

            return user;
        }
    }
}
