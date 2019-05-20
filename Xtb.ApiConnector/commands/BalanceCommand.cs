using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xAPI.Commands;

namespace SyncAPIConnect.Commands
{
    public class BalanceCommand: BaseCommand
    {
        public BalanceCommand(string streammingId, bool? prettyPrint) : base(prettyPrint, streammingId)
        {
        }
        //getBalance
        //getAccountIndicators
        public override string CommandName => "getBalance";

        public override string[] RequiredArguments => new string[] { };

        public override string ToString()
        {
            JObject result = new JObject();
            result.Add("command", "getBalance");
            result.Add("streamSessionId", streammingId);
            return result.ToString();
        }
    }
}
