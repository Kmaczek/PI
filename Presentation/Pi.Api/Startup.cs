using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Core.Common.Logging;
using Data.EF.Models;
using Data.EF.Models.Auth;
using Data.Repository;
using Data.Repository.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Pi.Api
{
    public partial class Startup
    {
        public Startup(IConfiguration configuration)
        {
            //var builder = new ConfigurationBuilder()
            //    .SetBasePath(Environment.Con)
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public ILifetimeScope AutofacContainer { get; private set; }


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            
            services.AddCors(options =>
            {
                options.AddPolicy("default", policy =>
                {
                    policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                });
            });

            services.AddCors();
        }

        public void ConfigureContainer(ContainerBuilder containerBuilder)
        {
            // wire up using autofac specific APIs here
        }

        //public void ConfigureContainer(ContainerBuilder diBuilder)
        //{
        //    diBuilder.RegisterModule<LoggingModule>();

        //    diBuilder.RegisterType<PiContext>();
        //    diBuilder.Register<Func<PiContext>>(x =>
        //    {
        //        var context = x.Resolve<IComponentContext>();
        //        return () => context.Resolve<PiContext>();
        //    });
        //    //diBuilder.RegisterType<BinanceRepository>().As<IBinanceRepository>();
        //    //diBuilder.RegisterType<OtoDomRepository>().As<IOtoDomRepository>();
        //    diBuilder.RegisterType<AppUser>().As<IAppUserRepository>();
        //}

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseRouting();

            app.UseCors("default");
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
