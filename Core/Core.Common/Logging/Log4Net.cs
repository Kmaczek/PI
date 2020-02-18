using log4net;
using System;

namespace Core.Common.Logging
{
    public class Log4Net : ILogger
    {
        private readonly ILog _log;
        private readonly Type type;

        public Log4Net(Type type)
        {
            _log = LogManager.GetLogger(type);
            this.type = type;
        }

        public void Error(string message)
        {
            _log.Error(message);
        }

        public void Error(string message, Exception exception)
        {
            _log.Error(message, exception);
        }

        public void Info(string message)
        {
            _log.Info(message);
        }
    }
}
