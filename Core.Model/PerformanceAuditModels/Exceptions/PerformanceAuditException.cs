using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Model.PerformanceAuditModels.Exceptions
{

    [Serializable]
    public class PerformanceAuditException : Exception
    {
        public PerformanceAuditException() { }
        public PerformanceAuditException(string message) : base(message) { }
        public PerformanceAuditException(string message, Exception inner) : base(message, inner) { }
        protected PerformanceAuditException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
