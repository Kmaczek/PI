using System;
using System.Collections.Generic;
using System.Text;
using xAPI.Records;
using xAPI.Responses;

namespace Xtb.Core
{
    public interface XtbInterface
    {
        StreamingBalanceRecord GetBalance();
        ServerTimeResponse GetServerTime();
    }
}
