﻿using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
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

        private static Server serverData = Servers.REAL;
        private static string userId = string.Empty;
        private static string password = string.Empty;
        private static string appName = string.Empty;
        private static string appGuid = (new Guid()).ToString();

        public XtbService(IConfigurationRoot configuration)
        {
            userId = configuration.GetSection(XtbUserId).Value;
            if (string.IsNullOrEmpty(userId))
                throw new XtbException($"Missing configuration for XTB service[{XtbUserId}]");

            password = configuration.GetSection(XtbPassword).Value;
            if (string.IsNullOrEmpty(password))
                throw new XtbException($"Missing configuration for XTB service[{XtbPassword}]");

            appName = configuration.GetSection("xtb:appName").Value;
            if (string.IsNullOrEmpty(appName))
                appName = "dkapp";
        }

        private StreamingBalanceRecord BalanceRecord { get; set; }

        public StreamingBalanceRecord GetBalance()
        {
            StreamingAPIConnector streamingApi = new StreamingAPIConnector(serverData);
            try
            {
                SyncAPIConnector connector = new SyncAPIConnector(serverData);
                Credentials credentials = new Credentials(userId, password, appGuid, appName);
                LoginResponse loginResponse = APICommandFactory.ExecuteLoginCommand(connector, credentials, true);

                streamingApi.StreamSessionId = loginResponse.StreamSessionId;
                streamingApi.Connect();
                streamingApi.SubscribeBalance();

                streamingApi.BalanceRecordReceived += OnBalanceChanged;

                var timestamp = DateTime.Now.Millisecond;
                var timeout = 5000;
                //wait for balance
                while (BalanceRecord == null)
                {
                    Thread.Sleep(50);
                    if (timestamp + timeout < DateTime.Now.Millisecond)
                        break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                streamingApi.UnsubscribeBalance();
                streamingApi.Disconnect();
                streamingApi = null;
            }

            return BalanceRecord;
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