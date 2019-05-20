using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xAPI.Responses;

namespace xAPI.Responses
{
    using JSONObject = Newtonsoft.Json.Linq.JObject;
    public class BalanceResponse: BaseResponse
    {
        //{ balance = 9031,81, margin = 2653,56, marginFree = 6354,54, marginLevel = 339,47, equity = 9008,1, credit = 0}

        public BalanceResponse(string body) : base(body)
        {
            JSONObject ob = (JSONObject)this.ReturnData;
            //this.time = (long?)ob["time"];
            //this.timeString = (string)ob["timeString"];
            this.Balance = (double?)ob["balance"];
        }

        public double? Balance { get; set; }
    }
}
