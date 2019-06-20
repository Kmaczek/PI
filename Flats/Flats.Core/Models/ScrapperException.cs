using System;
using System.Collections.Generic;
using System.Text;

namespace Flats.Core.Models
{

    [Serializable]
    public class ScrapperException : Exception
    {
        public ScrapperException() { }
        public ScrapperException(string message) : base(message) { }
        public ScrapperException(string message, Exception inner) : base(message, inner) { }
        protected ScrapperException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
