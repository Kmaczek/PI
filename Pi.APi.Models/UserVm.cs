using Pi.Api.EF.Models.Auth;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pi.APi.Models
{
    public class UserVm
    {
        public int Id { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public DateTime? LastLoginUtc { get; set; }

        public int[] UserRoles { get; set; } = [];

        public static UserVm FromDm(AppUserDm user, IEnumerable<RoleDm> roles = null)
        {
            return new UserVm
            {
                Id = user.Id,
                DisplayName = user.DisplayName,
                Email = user.Email,
                LastLoginUtc = user.LastLoginUtc,
                UserRoles = roles != null ? roles.Select(r => r.Id).ToArray() : user.UserRoles.Select(r => r.Role.Id).ToArray()
            };
        }
    }
}
