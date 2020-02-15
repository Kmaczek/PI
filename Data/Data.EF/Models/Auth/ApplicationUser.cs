using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.EF.Models.Auth
{
    public class AppUser
    {
        public AppUser()
        {
        }

        public int Id { get; set; }
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public string Password { get; set; }
        public DateTime ActiveFrom { get; set; }
        public DateTime ActiveTo { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
