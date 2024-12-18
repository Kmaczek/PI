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
        public string DisplayName { get; set; }
        public byte[] Password { get; set; }
        public byte[] Salt { get; set; }
        public string Email { get; set; }
        public DateTime? ActiveFromUtc { get; set; }
        public DateTime? ActiveToUtc { get; set; }
        public DateTime CreatedDateUtc { get; set; }
        public DateTime? LastLoginUtc { get; set; }

        public virtual ICollection<UserRoleDm> UserRoles { get; set; }
    }
}
