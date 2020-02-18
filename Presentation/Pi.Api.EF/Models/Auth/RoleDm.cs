using System.Collections.Generic;

namespace Pi.Api.EF.Models.Auth
{
    public class RoleDm
    {
        public RoleDm()
        {
            UserRoles = new HashSet<UserRoleDm>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<UserRoleDm> UserRoles { get; set; }
    }
}
