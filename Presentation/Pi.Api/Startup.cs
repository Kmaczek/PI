using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Core.Common.Logging;
using Core.Domain.Logic;
using Data.EF.Models;
using Data.Repository;
using Data.Repository.Interfaces;
using log4net;
using log4net.Config;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Pi.Api.EF;
using Pi.Api.EF.Repository.Interfaces;
using Pi.Api.Middleware;
using Pi.Api.Services;
using Pi.Api.Services.Interfaces;

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

            SetupLogger();
            IdentityModelEventSource.ShowPII = true;
        }

        public IConfiguration Configuration { get; }

        public ILifetimeScope AutofacContainer { get; private set; }

        public ILog Log { get; private set; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddLogging();

            services.AddAuthentication(SetJwtBearerAuthentication)
                .AddJwtBearer(SetJwtBearerOptions);
            services.AddAuthorization();

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
            diBuilder.RegisterType<ApiContext>();
            diBuilder.Register<Func<ApiContext>>(x =>
            {
                var context = x.Resolve<IComponentContext>();
                return () => context.Resolve<ApiContext>();
            });

            diBuilder.RegisterType<PiContext>();
            diBuilder.Register<Func<PiContext>>(x =>
            {
                var context = x.Resolve<IComponentContext>();
                return () => context.Resolve<PiContext>();
            });

            diBuilder.RegisterType<AppUserRepository>().As<IAppUserRepository>();
            diBuilder.RegisterType<OtoDomRepository>().As<IOtoDomRepository>();
            diBuilder.RegisterType<PriceRepository>().As<IPriceRepository>();

            diBuilder.RegisterType<ApiConfig>().As<IApiConfig>();
            diBuilder.RegisterType<TokenService>().As<ITokenService>();
            diBuilder.RegisterType<PasswordHashingService>().As<IPasswordHashing>();
            diBuilder.RegisterType<UserService>().As<IUserService>();
            diBuilder.RegisterType<FlatSeriesService>().As<IFlatSeriesService>();
            diBuilder.RegisterType<PriceService>().As<IPriceService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            AutofacContainer = app.ApplicationServices.GetAutofacRoot();

            app.UseMiddleware(typeof(ErrorHandlingMiddleware), Log);
            //app.UseExceptionHandler(a => a.Run(async context =>
            //{
            //    var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
            //    var exception = exceptionHandlerPathFeature.Error;
            //    Log.Error("a", exception);

            //    var result = JsonConvert.SerializeObject(new { error = exception.Message });
            //    context.Response.ContentType = "application/json";
            //    await context.Response.WriteAsync(result);
            //}));

            app.UseRouting();

            app.UseCors("default");
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void SetupLogger()
        {
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));

            Log = LogManager.GetLogger(typeof(Startup));
        }

        private void SetJwtBearerAuthentication(AuthenticationOptions options)
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }

        private void SetJwtBearerOptions(JwtBearerOptions options)
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;

            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuerSigningKey = false,
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidIssuer = Configuration["Jwt:Issuer"],
                ValidAudience = Configuration["Jwt:Issuer"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetSection("PI_TokenPrivateKey").Value))
            };

            options.Events = new JwtBearerEvents();
            options.Events.OnAuthenticationFailed += new Func<AuthenticationFailedContext, Task>((context) =>
            {
                Log.Debug($"Failed to authorize: {context.Exception}");
                return Task.FromResult(context.Result);
            });
        }
    }
}
