using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Autofac.Extensions.DependencyInjection;
using Autofac;
using Data.EF.Models;
using Data.EF.Models.Auth;
using Data.Repository.Interfaces;
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
                //.ConfigureContainer<ContainerBuilder>(diBuilder =>
                //{
                //    diBuilder.RegisterType<PiContext>();
                //    diBuilder.RegisterType<AppUser>().As<IAppUserRepository>();
                //    //builder.RegisterType
                //})
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureKestrel(serverOptions =>
                    {
                        serverOptions.Listen(IPAddress.Loopback, 5001);
                        serverOptions.Listen(IPAddress.Any, 80);
                        serverOptions.Listen(IPAddress.Loopback, 443, listenOprions =>
                        {
                            listenOprions.UseHttps();
                        });

                    }).UseStartup<Startup>();
                });
    }
}
