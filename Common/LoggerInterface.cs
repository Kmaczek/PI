using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Common
{
    public interface LoggerInterface
    {
        void Info(string message);

        void Error(string message);
        void Error(string message, Exception exception);
    }
}
