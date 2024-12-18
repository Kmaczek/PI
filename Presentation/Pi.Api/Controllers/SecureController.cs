using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;

namespace Pi.Api.Controllers
{
    [Authorize]
    public class SecureController : ControllerBase
    {
        private int? userId;
        protected int UserId
        {
            get
            {
                if (userId == null)
                {
                    var sub = User.FindFirstValue("sub") ?? throw new Exception("invalid_token");
                    userId = int.Parse(sub);
                }

                return userId.Value;
            }
        }
    }
}
