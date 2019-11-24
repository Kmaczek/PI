using Core.Common;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using xAPI.Codes;
using xAPI.Commands;
using xAPI.Records;
using xAPI.Responses;
using xAPI.Sync;
using Xtb.Core.Models;

namespace Xtb.Core
{
    public class XtbService : XtbInterface
    {
        private const string XtbUserId = "PI_XtbUserId";
        private const string XtbPassword = "PI_XtbPassword";

        private readonly ILogger log;
        private static Server serverData = Servers.REAL;
        private static string userId = string.Empty;
        private static string password = string.Empty;
        private static string appName = string.Empty;
        private static int timeout = 5000;
        private static string appGuid = (new Guid()).ToString();

        public XtbService(
            ILogger log,
            IConfigurationRoot configuration)
        {
            if(configuration == null)
                throw new XtbException($"Missing configuration provider for XTB service.");

            userId = configuration.GetSection(XtbUserId).Value;
            if (string.IsNullOrEmpty(userId))
                throw new XtbException($"Missing configuration for XTB service[{XtbUserId}]");

            password = configuration.GetSection(XtbPassword).Value;
            if (string.IsNullOrEmpty(password))
                throw new XtbException($"Missing configuration for XTB service[{XtbPassword}]");

            appName = configuration.GetSection("xtb:appName").Value;
            if (string.IsNullOrEmpty(appName))
                appName = "dkapp";

            timeout = Convert.ToInt32(configuration.GetSection("xtb:timeout").Value);

            this.log = log;
        }

        private StreamingBalanceRecord BalanceRecord { get; set; }

        public StreamingBalanceRecord GetBalance()
        {
            BalanceRecord = null;
            using (StreamingAPIConnector streamingApi = new StreamingAPIConnector(serverData))
            using (SyncAPIConnector connector = new SyncAPIConnector(serverData))
            {
                try
                {
                    Credentials credentials = new Credentials(userId, password, appGuid, appName);
                    LoginResponse loginResponse = APICommandFactory.ExecuteLoginCommand(connector, credentials, true);

                    streamingApi.StreamSessionId = loginResponse.StreamSessionId;
                    streamingApi.Connect();
                    streamingApi.SubscribeBalance();

                    streamingApi.BalanceRecordReceived += OnBalanceChanged;

                    Stopwatch sw = new Stopwatch();
                    sw.Start();

                    var timestamp = sw.ElapsedMilliseconds;
                    
                    //wait for balance
                    while (BalanceRecord == null)
                    {
                        Thread.Sleep(50);
                        if (timestamp + timeout < sw.ElapsedMilliseconds)
                        {
                            sw.Stop();
                            log.Info($"Unable to fetch balance, timeout of [{timeout}s] elapsed.");
                            break;
                        }
                    }
                }
                catch (Exception e)
                {
                    log.Error("Unable to fetch balance, exception occured.", e);
                }
                finally
                {
                    streamingApi.UnsubscribeBalance();
                }

                return BalanceRecord;
            }
        }

        public ServerTimeResponse GetServerTime()
        {
            SyncAPIConnector connector = new SyncAPIConnector(serverData);
            LoginResponse loginResponse = APICommandFactory.ExecuteLoginCommand(connector, GetCredentials(), true);
            ServerTimeResponse serverTimeResponse = APICommandFactory.ExecuteServerTimeCommand(connector, true);

            return serverTimeResponse;
        }

        public AllSymbolsResponse GetAllSymbols()
        {
            SyncAPIConnector connector = new SyncAPIConnector(serverData);
            LoginResponse loginResponse = APICommandFactory.ExecuteLoginCommand(connector, GetCredentials(), true);
            var allSymbolsResponse = APICommandFactory.ExecuteAllSymbolsCommand(connector, true);

            return allSymbolsResponse;
        }

        // Prototype
        public ChartLastResponse GetChart(string symbol)
        {
            if (string.IsNullOrEmpty(symbol))
            {
                symbol = "OIL"; //default to oil, change this later
            }

            SyncAPIConnector connector = new SyncAPIConnector(serverData);
            LoginResponse loginResponse = APICommandFactory.ExecuteLoginCommand(connector, GetCredentials(), true);
            var ms = DateTimeToUnixTimestamp(DateTime.Now.AddDays(-1));
            var chart = APICommandFactory.ExecuteChartLastCommand(connector, new ChartLastInfoRecord("OIL", PERIOD_CODE.PERIOD_M30, ms));

            return chart;
        }

        // Prototype
        public TickPricesResponse GetTicks(string symbol)
        {
            if (string.IsNullOrEmpty(symbol))
            {
                symbol = "OIL"; //default to oil, change this later
            }

            SyncAPIConnector connector = new SyncAPIConnector(serverData);
            LoginResponse loginResponse = APICommandFactory.ExecuteLoginCommand(connector, GetCredentials(), true);
            var ms = DateTimeToUnixTimestamp(DateTime.Now.AddDays(-1));
            var ticks = APICommandFactory.ExecuteTickPricesCommand(connector, new List<string> { "OIL" }, DateTime.Now.Millisecond);

            return ticks;
        }

        private Credentials GetCredentials()
        {
            var credentials = new Credentials(userId, password, appGuid, appName);
            return credentials;
        }

        private void OnBalanceChanged(StreamingBalanceRecord balanceRecord)
        {
            BalanceRecord = balanceRecord;
        }

        public static long DateTimeToUnixTimestamp(DateTime dateTime)
        {
            return (long)(TimeZoneInfo.ConvertTimeToUtc(dateTime) -
                   new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc)).TotalMilliseconds;
        }
    }
}
