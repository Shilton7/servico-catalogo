using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace CatalogoAPI.Logging
{
    public class CustomerLogger : ILogger
    {
        readonly string _loggerName;
        readonly CustomLoggerProviderConfig _loggerConfig;
        readonly IConfiguration _configuration;

        public CustomerLogger(string name, 
                              CustomLoggerProviderConfig config, 
                              IConfiguration configuration)
        {
            _loggerName = name;
            _loggerConfig = config;
            _configuration = configuration;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel == _loggerConfig.LogLevel;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state,
            Exception exception, Func<TState, Exception, string> formatter)
        {
            string mensagem = $"{logLevel.ToString()}: {eventId.Id} - {formatter(state, exception)}";

            EscreverTextoNoArquivo(mensagem);
        }

        private void EscreverTextoNoArquivo(string mensagem)
        {
            string caminhoArquivoLog = _configuration["Logging_path_file"];
            using (StreamWriter streamWriter = new StreamWriter(caminhoArquivoLog, true))
            {
                try
                {
                    streamWriter.WriteLine(mensagem);
                    streamWriter.Close();
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }
}
