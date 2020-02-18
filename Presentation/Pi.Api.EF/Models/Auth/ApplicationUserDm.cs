using System;
using System.Collections.Generic;

namespace Pi.Api.EF.Models.Auth
{
    public class AppUserDm
    {
        public AppUserDm()
        {
            UserRoles = new HashSet<UserRoleDm>();
        }

        public int Id { get; set; }
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public byte[] Password { get; set; }
        public byte[] Salt { get; set; }
        public string Email { get; set; }
        public DateTime ActiveFrom { get; set; }
        public DateTime ActiveTo { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual ICollection<UserRoleDm> UserRoles { get; set; }
    }
}
