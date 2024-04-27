using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Common.Logging
{
    public interface ILogger
    {
        void Info(string message);

        void Error(string message);
        void Error(string message, Exception exception);
    }
}
