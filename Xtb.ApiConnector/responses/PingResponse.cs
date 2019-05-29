namespace xAPI.Responses
{
	using JSONObject = Newtonsoft.Json.Linq.JObject;

    public class PingResponse : BaseResponse
	{
		public PingResponse(string body) : base(body)
		{
			JSONObject ob = (JSONObject) this.ReturnData;
		}
	}
}