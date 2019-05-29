using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Common
{
    public interface IEmailService
    {
        void SendEmail(string body, string title = null);
    }
}
