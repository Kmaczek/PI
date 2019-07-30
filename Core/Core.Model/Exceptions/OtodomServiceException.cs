using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Model.Exceptions
{

    [Serializable]
    public class OtodomServiceException : Exception
    {
        public OtodomServiceException() { }
        public OtodomServiceException(string message) : base(message) { }
        public OtodomServiceException(string message, Exception inner) : base(message, inner) { }
        protected OtodomServiceException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
