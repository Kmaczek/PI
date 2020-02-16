using System;
using System.IO;
using Autofac;
using Core.Common;
using Core.Common.Logging;
using Data.EF.Models;
using Data.Repository;
using Data.Repository.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Pi.Api
{
    public class Startup
    {
        private static IConfigurationRoot _configuration;

        public Startup(IConfiguration configuration)
        {
            var builder = new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("appSettings.json", optional: true, reloadOnChange: true)
                            .AddEnvironmentVariables();

            _configuration = builder.Build();
            Configuration = _configuration;
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

        public void ConfigureContainer(ContainerBuilder diBuilder)
        {
            diBuilder.RegisterModule<LoggingModule>();

            diBuilder.RegisterInstance(_configuration).SingleInstance();
            //diBuilder.RegisterType<ConfigHelper>().SingleInstance();
            diBuilder.RegisterType<PiContext>();
            diBuilder.Register<Func<PiContext>>(x =>
            {
                var context = x.Resolve<IComponentContext>();
                return () => context.Resolve<PiContext>();
            });

            diBuilder.RegisterType<AppUserRepository>().As<IAppUserRepository>();
        }

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
