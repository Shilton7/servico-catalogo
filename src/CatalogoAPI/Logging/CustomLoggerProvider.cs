using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;

namespace CatalogoAPI.Logging
{
    public class CustomLoggerProvider : ILoggerProvider
    {
        readonly IConfiguration _configuration;
        private readonly CustomLoggerProviderConfig _loggerProviderConfig;
        private readonly ConcurrentDictionary<string, CustomerLogger> loggers =
            new ConcurrentDictionary<string, CustomerLogger>();

        public CustomLoggerProvider(CustomLoggerProviderConfig config, 
            IConfiguration configuration)
        {
            _loggerProviderConfig = config;
            _configuration = configuration;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return loggers.GetOrAdd(categoryName, name => new CustomerLogger(name, _loggerProviderConfig, _configuration));
        }

        public void Dispose()
        {
            loggers.Clear();
        }
    }
}
