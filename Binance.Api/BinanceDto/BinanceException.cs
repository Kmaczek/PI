using System;
using System.Collections.Generic;
using System.Text;

namespace Binance.Api.BinanceDto
{
    [Serializable]
    public class BinanceException : Exception
    {
        public BinanceException() { }
        public BinanceException(string message) : base(message) { }
        public BinanceException(string message, Exception inner) : base(message, inner) { }
        protected BinanceException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
