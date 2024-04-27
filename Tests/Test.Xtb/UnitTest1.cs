using Autofac;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using System.IO;
using Xtb.Core;

namespace Test.Xtb
{
    public class Tests
    {
        public static IContainer InjectionContainer;
        public static IConfigurationRoot Configuration;
        private XtbInterface _xtbService;

        [SetUp]
        public void Setup()
        {
            var builder = new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("appSettings.json", optional: true, reloadOnChange: true)
                            .AddEnvironmentVariables();
            Configuration = builder.Build();

            BuildInjectionContainer();
            _xtbService = InjectionContainer.Resolve<XtbInterface>();
        }

        [Test]
        public void Test1()
        {
            var balance = _xtbService.GetBalance();

            Assert.AreNotEqual(0, balance);
        }

        private static void BuildInjectionContainer()
        {
            var diBuilder = new ContainerBuilder();
            diBuilder.RegisterInstance(Configuration).SingleInstance();

            diBuilder.RegisterType<XtbService>().As<XtbInterface>();
            InjectionContainer = diBuilder.Build();
        }
    }
}