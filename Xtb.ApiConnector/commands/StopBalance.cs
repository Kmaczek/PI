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
        string streammingId;

        public StopBalance(string streammingId, bool? prettyPrint) : base(prettyPrint)
        {
            this.streammingId = streammingId;
        }

        public override string CommandName => "stopBalance";

        public override string[] RequiredArguments => new string[] { };

        public override string ToString()
        {
            JObject result = new JObject();
            result.Add("command", "getBalance");
            //result.Add("streamSessionId", streammingId);
            return result.ToString();
        }
    }
}
