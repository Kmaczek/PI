using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Common
{
    public interface EmailServiceInterface
    {
        void SendEmail(string body, string title = null);
    }
}
