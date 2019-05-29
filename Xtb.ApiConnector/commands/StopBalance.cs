using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xAPI.Commands;
using Newtonsoft.Json.Linq;

namespace SyncAPIConnect.Commands
{
    public class StopBalance : BaseCommand
    {
        public StopBalance(string streammingId, bool? prettyPrint) : base(prettyPrint)
        {
            this.streammingId = streammingId;
        }

        public override string CommandName => "stopBalance";

        public override string[] RequiredArguments => new string[] { };

        public override string ToString()
        {
            JObject result = new JObject
            {
                { "command", "getBalance" }
            };
            return result.ToString();
        }
    }
}
