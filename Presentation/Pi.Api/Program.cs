using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Autofac.Extensions.DependencyInjection;
using System.Net;

namespace Pi.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureKestrel(serverOptions =>
                    {
                        //serverOptions.Listen(IPAddress.Loopback, 5001);
                        //serverOptions.Listen(IPAddress.Parse("116.203.100.224"), 5001);
                        serverOptions.Listen(IPAddress.Any, 5001);
                        //serverOptions.Listen(IPAddress.Any, 8080);
                        //serverOptions.Listen(IPAddress.Loopback, 443, listenOprions =>
                        //{
                        //    listenOprions.UseHttps();
                        //});

                    }).UseStartup<Startup>();
                    webBuilder.UseKestrel();
                });
    }
}
