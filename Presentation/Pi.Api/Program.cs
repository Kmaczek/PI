using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Autofac.Extensions.DependencyInjection;
using Autofac;
using Data.EF.Models;
using Data.EF.Models.Auth;
using Data.Repository.Interfaces;

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
                //.UseServiceProviderFactory(new AutofacServiceProviderFactory())
                //.ConfigureContainer<ContainerBuilder>(diBuilder =>
                //{
                //    diBuilder.RegisterType<PiContext>();
                //    diBuilder.RegisterType<AppUser>().As<IAppUserRepository>();
                //    //builder.RegisterType
                //})
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
