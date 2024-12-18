using Autofac;
using Autofac.Extensions.DependencyInjection;
using Core.Common.Logging;
using Core.Domain.Logic;
using Core.Domain.Logic.Chatbot;
using Data.EF.Models;
using Data.Repository;
using Data.Repository.Interfaces;
using HealthChecks.UI.Client;
using log4net;
using log4net.Config;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Pi.Api.EF;
using Pi.Api.EF.Repository;
using Pi.Api.EF.Repository.Interfaces;
using Pi.Api.HealthChecks;
using Pi.Api.Middleware;
using Pi.Api.Services;
using Pi.Api.Services.Interfaces;
using Pi.Api.Swagger;
using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Pi.Api
{
    public class Startup
    {
        private static IConfigurationRoot _configuration;
        private readonly IWebHostEnvironment _env;
        private ILogger<Startup> _logger;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("appSettings.json", optional: true, reloadOnChange: true)
                            .AddEnvironmentVariables();

            _configuration = builder.Build();
            _env = env;
            Configuration = _configuration;

            SetupLogger(env);
            IdentityModelEventSource.ShowPII = true;
        }

        public IConfiguration Configuration { get; }

        public ILifetimeScope AutofacContainer { get; private set; }

        public ILog Log { get; private set; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddLogging(logging =>
            {
                logging.AddLog4Net();
                logging.SetMinimumLevel(LogLevel.Debug);
            });

            services.AddAuthentication(SetJwtBearerAuthentication)
                .AddJwtBearer(SetJwtBearerOptions);
            services.AddAuthorization();

            services.AddHealthChecks()
                .AddCheck("self", () => HealthCheckResult.Healthy(), tags: ["api"])
                .AddCheck<EfContextHealthCheck>("EF context");

            services.AddCors(options =>
            {
                options.AddPolicy("default", policy =>
                {
                    policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                });
            });

            services.AddCors();

            if (!_env.IsProduction())
            {
                services.AddSwaggerForPi();
            }
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
            diBuilder.RegisterType<TransactionClassifier>().As<ITransactionClassifier>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            _logger = logger;
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Pi v1");
                    c.RoutePrefix = "swagger";
                });
            }
            AutofacContainer = app.ApplicationServices.GetAutofacRoot();


            app.UseErrorHandling();
            app.UsePathBase("/api");
            app.UseRouting();
            app.UseCors("default");

            // clearing default ms maps, so 'sub' claim will work
            Microsoft.IdentityModel.JsonWebTokens.JsonWebTokenHandler.DefaultInboundClaimTypeMap.Clear();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health", new HealthCheckOptions
                {
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
            });
        }

        private void SetupLogger(IWebHostEnvironment environment)
        {
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            var configFile = new FileInfo(Path.Combine(environment.ContentRootPath, "log4net.config"));

            if (!configFile.Exists)
            {
                throw new FileNotFoundException("log4net.config not found", configFile.FullName);
            }

            XmlConfigurator.Configure(logRepository, configFile);
            Console.WriteLine("Logging initialized successfully");
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
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = false,
                ValidIssuer = Configuration["Jwt:Issuer"],
                ValidAudience = Configuration["Jwt:Issuer"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetSection("PI_TokenPrivateKey").Value)),
                RoleClaimType = "role"
            };

            options.Events = new JwtBearerEvents();
            options.Events.OnAuthenticationFailed += new Func<AuthenticationFailedContext, Task>((context) =>
            {
                _logger.LogDebug($"Failed to authorize: {context.Exception}");
                return Task.FromResult(context.Result);
            });
        }
    }
}
