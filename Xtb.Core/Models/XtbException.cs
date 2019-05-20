using System;
using System.Collections.Generic;
using System.Text;

namespace Xtb.Core.Models
{

    [Serializable]
    public class XtbException : Exception
    {
        public XtbException() { }
        public XtbException(string message) : base(message) { }
        public XtbException(string message, Exception inner) : base(message, inner) { }
        protected XtbException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
