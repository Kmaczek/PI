using Binance.Api;
using Core.Common;
using Core.Domain.Logic;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using System.IO;

namespace Tests
{
    public class BinanceClientTests
    {
        private static IConfigurationRoot Configuration;
        private static readonly ILogger Log = new Log4Net(typeof(BinanceClientTests));

        [SetUp]
        public void Setup()
        {
            SetupConfig();
        }

        [Test]
        public void Test1()
        {
            var binanceClient = new BinanceClient(Configuration);

            var bs = new BinanceService(binanceClient, Log);
            var accInfo = bs.GetSymbolValuesForAccount();

            Assert.Pass();
        }

        private static void SetupConfig()
        {
            var builder = new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("appSettings.json", optional: true, reloadOnChange: true)
                            .AddEnvironmentVariables();
            Configuration = builder.Build();
        }
    }
}