using RestSharp;
using System.Security.Cryptography;
using System;
using System.Text;
using Newtonsoft.Json;
using Binance.Api.BinanceDto;
using Newtonsoft.Json.Serialization;
using Microsoft.Extensions.Configuration;
using System.Globalization;

namespace Binance.Api
{
    public class BinanceClient : IBinanceClient
    {
        private const string BinanceApiKey = "PI_BinanceApiKey";
        private const string BinanceApiSecret = "PI_BinanceApiSecret";

        private readonly string url;
        private readonly string secret;
        private readonly string apiKey;
        private RestClient client;

        public RestClient Client
        {
            get
            {
                if (client == null)
                {
                    client = new RestClient(url);
                }

                return client;
            }
        }

        public JsonSerializerSettings CamelCaseResolver
        {
            get
            {
                return new JsonSerializerSettings()
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                };
            }
        }

        public BinanceClient(IConfigurationRoot configuration)
        {
            url = configuration.GetSection("binance:url").Value;

            apiKey = configuration.GetSection(BinanceApiKey).Value;
            if (string.IsNullOrEmpty(apiKey))
                throw new BinanceException($"Missing configuration for Binance service[{BinanceApiKey}]");

            secret = configuration.GetSection(BinanceApiSecret).Value;
            if (string.IsNullOrEmpty(secret))
                throw new BinanceException($"Missing configuration for Binance service[{BinanceApiSecret}]");
        }

        public bool Ping()
        {
            var request = new RestRequest("/api/v1/ping", Method.GET);
            IRestResponse response = Client.Execute(request);

            return response.StatusCode == System.Net.HttpStatusCode.OK;
        }

        public ServerTimeDto ServerTime()
        {
            var request = new RestRequest("/api/v1/time", Method.GET);
            IRestResponse response = Client.Execute(request);

            var serverTime = JsonConvert.DeserializeObject<ServerTimeDto>(response.Content);
            return serverTime;
        }

        public ExchangeInfoDto ExchangeInfo()
        {
            var request = new RestRequest("/api/v1/exchangeInfo", Method.GET);
            IRestResponse response = Client.Execute(request);

            var exchangeInfo = JsonConvert.DeserializeObject<ExchangeInfoDto>(response.Content, CamelCaseResolver);
            return exchangeInfo;
        }

        public ExchangeInfoDto OrderBook()
        {
            var request = new RestRequest("/api/v1/depth", Method.GET);
            IRestResponse response = Client.Execute(request);

            var exchangeInfo = JsonConvert.DeserializeObject<ExchangeInfoDto>(response.Content, CamelCaseResolver);
            return exchangeInfo;
        }

        public TradeDto HistoricalTrades()
        {
            var request = new RestRequest("/api/v1/historicalTrades", Method.GET);
            IRestResponse response = Client.Execute(request);

            var trade = JsonConvert.DeserializeObject<TradeDto>(response.Content, CamelCaseResolver);
            return trade;
        }

        public BinanceResponse<AccountInformationDto> AccountInfo()
        {
            var response = new BinanceResponse<AccountInformationDto>()
            {
                Status = OutcomeStatus.OK
            };

            var request = new RestRequest("/api/v3/account", Method.GET);
            request.AddHeader("X-MBX-APIKEY", apiKey);
            request.AddOrUpdateParameter(new Parameter() { Name = "timestamp", Value = ServerTime().ServerTimestamp, Type = ParameterType.QueryString });

            AddSignature(request);
            IRestResponse requestResponse = Client.Execute(request);

            if (!requestResponse.IsSuccessful)
            {
                response.Status = OutcomeStatus.Failed;
                response.Notifications.Add(Notification.AddError($"Status {requestResponse.StatusCode}: {requestResponse.StatusDescription}"));
                var error = JsonConvert.DeserializeObject<ErrorMessageDto>(requestResponse.Content, CamelCaseResolver);
                response.Notifications.Add(Notification.AddError($"Error: {error.Code} - {error.Msg}"));

                return response;
            }

            var trade = JsonConvert.DeserializeObject<AccountInformationDto>(requestResponse.Content, CamelCaseResolver);
            response.ResponseDto = trade;

            return response;
        }

        public AveragePriceDto GetAveragePrice(string symbol)
        {
            var request = new RestRequest("/api/v3/avgPrice", Method.GET);
            request.Parameters.Add(new Parameter("symbol", symbol, ParameterType.QueryString));
            IRestResponse response = Client.Execute(request);

            var avgPrice = JsonConvert.DeserializeObject<AveragePriceDto>(response.Content, CamelCaseResolver);
            avgPrice.Symbol = symbol;

            return avgPrice;
        }

        private void AddSignature(RestRequest request)
        {
            var uri = Client.BuildUri(request);
            var query = uri.Query.Trim('?');
            request.AddOrUpdateParameter("signature", HmacSha256Digest(query, secret), ParameterType.QueryString);
        }

        public static void Test()
        {
            var query = "symbol=LTCBTC&side=BUY&type=LIMIT&timeInForce=GTC&quantity=1&price=0.1&recvWindow=5000&timestamp=1499827319559";
            var sec = "NhqPtmdSJYdKjVHjA7PZj4Mge3R5YNiP1e3UZjInClVN65XAbvqqM6A7H5fATj0j";
            var outp = "c8db56825ae71d6d79447849e617115f4a920fa2acdcab2b053c4b2838bd6b71";
            var sign = HmacSha256Digest(query, sec);

            var isOk = sign == outp;
        }

        public static string HmacSha256Digest(string message, string secret)
        {
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] keyBytes = encoding.GetBytes(secret);
            byte[] messageBytes = encoding.GetBytes(message);
            HMACSHA256 cryptographer = new HMACSHA256(keyBytes);

            byte[] bytes = cryptographer.ComputeHash(messageBytes);

            return BitConverter.ToString(bytes)
                .Replace("-", "", StringComparison.Ordinal)
                .ToLower(CultureInfo.InvariantCulture);
        }
    }
}
