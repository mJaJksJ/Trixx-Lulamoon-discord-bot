using Serilog;

namespace Trixx.Common.Services
{
    public class LoggerService
    {
        private readonly ILogger _logger;

        public LoggerService(ILogger logger)
        {
            _logger = logger;
        }

        public void Error(string error, Exception? exception = null)
        {
            if (exception == null)
            {
                _logger.Error(error);
            }
            else
            {
                _logger.Error(error, exception);
            }
        }

        public void Info(string info)
        {
            _logger.Information(info);
        }
    }
}
