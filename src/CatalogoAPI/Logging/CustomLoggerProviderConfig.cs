using Microsoft.Extensions.Logging;

namespace CatalogoAPI.Logging
{
    public class CustomLoggerProviderConfig
    {
        public int EventId { get; set; } = 0;
        public LogLevel LogLevel { get; set; } = LogLevel.Warning;
    }
}
