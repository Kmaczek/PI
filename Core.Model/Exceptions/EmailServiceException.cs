using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Model.Exceptions
{

    [Serializable]
    public class EmailServiceException : Exception
    {
        public EmailServiceException() { }
        public EmailServiceException(string message) : base(message) { }
        public EmailServiceException(string message, Exception inner) : base(message, inner) { }
        protected EmailServiceException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
