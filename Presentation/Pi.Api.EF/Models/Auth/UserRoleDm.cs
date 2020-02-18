using System;
using System.Collections.Generic;
using System.Text;

namespace Pi.Api.EF.Models.Auth
{
    public class UserRoleDm
    {
        public UserRoleDm()
        {

        }

        public int Id { get; set; }

        public int UserId { get; set; }
        public virtual AppUserDm User { get; set; }

        public int RoleId { get; set; }
        public virtual RoleDm Role { get; set; }
    }
}
