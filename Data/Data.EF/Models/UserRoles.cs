using System;
using System.Collections.Generic;

namespace Data.EF.Models
{
    public partial class UserRoles
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int RoleId { get; set; }

        public virtual Role Role { get; set; }
        public virtual User User { get; set; }
    }
}
