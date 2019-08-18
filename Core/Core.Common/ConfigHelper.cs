using Core.Model.Config;
using Core.Model.Exceptions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;

namespace Core.Common
{
    public class ConfigHelper
    {
        private readonly IConfigurationRoot config;

        public OtodomFeedJobConfig OtodomFeedJobConfig { get; private set; }

        public ConfigHelper(
            IConfigurationRoot config)
        {
            if (config == null)
            {
                throw new ConfigurationException($"Cant read IConfigurationRoot. Make sure it is properly injected.");
            }
            this.config = config;

            LoadConfig();
        }

        public void LoadConfig()
        {
            try
            {
                var jsonProvider = config.Providers.First(x => x is JsonConfigurationProvider) as JsonConfigurationProvider;
                var physicalFileProvider = jsonProvider.Source.FileProvider as PhysicalFileProvider;
                var path = physicalFileProvider.Root + jsonProvider.Source.Path;

                var configText = File.ReadAllText(path);
                var appConfig = JsonConvert.DeserializeObject<AppConfig>(configText);
                OtodomFeedJobConfig = appConfig.OtodomFeedJob;

                var s = new OtodomFeedJobConfig();
            }
            catch (ArgumentNullException e)
            {
                throw new ConfigurationException("Can't load Json configuration.", e);
            }
        }
    }
}
