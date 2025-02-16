using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text;
using TechChallenge3.Common.HttpSettings;

namespace TechChallenge3.Common.LogSettings
{
    public class Graylogger : IGraylogger
    {
        private readonly IHttpClient _httpClient;
        private readonly ILogger<Graylogger> _logger;
        private readonly GrayloggerSettings _grayloggerSettings;

        public Graylogger(
            IHttpClient httpClient,
            IConfiguration configuration,
            ILogger<Graylogger> logger)
        {
            _grayloggerSettings = configuration?.GetSection(nameof(GrayloggerSettings)).Get<GrayloggerSettings>() ?? throw new ArgumentNullException(nameof(GrayloggerSettings));
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task LogDebug(string logMessage)
        {
            _logger.LogDebug(logMessage);
            await SendGraylogRequest(logMessage, (int)LogLevel.Debug);
        }

        public async Task LogDebug(Exception? exception, string? logMessage)
        {
            _logger.LogDebug(exception, logMessage);
            await SendGraylogRequest(exception, logMessage, (int)LogLevel.Debug);
        }

        public async Task LogInformation(string logMessage)
        {
            _logger.LogInformation(logMessage);
            await SendGraylogRequest(logMessage, (int)LogLevel.Information);
        }

        public async Task LogInformation(Exception? exception, string logMessage)
        {
            _logger.LogDebug(exception, logMessage);
            await SendGraylogRequest(exception, logMessage, (int)LogLevel.Information);
        }

        public async Task LogWarning(string logMessage)
        {
            _logger.LogDebug(logMessage);
            await SendGraylogRequest(logMessage, (int)LogLevel.Warning);
        }

        public async Task LogWarning(Exception? exception, string logMessage)
        {
            _logger.LogDebug(exception, logMessage);
            await SendGraylogRequest(exception, logMessage, (int)LogLevel.Warning);
        }

        public async Task LogError(string logMessage)
        {
            _logger.LogDebug(logMessage);
            await SendGraylogRequest(logMessage, (int)LogLevel.Error);
        }

        public async Task LogError(Exception? exception, string logMessage)
        {
            _logger.LogError(exception, logMessage);
            await SendGraylogRequest(exception, logMessage, (int)LogLevel.Error);
        }

        public async Task LogCritical(string logMessage)
        {
            _logger.LogDebug(logMessage);
            await SendGraylogRequest(logMessage, (int)LogLevel.Critical);
        }

        public async Task LogCritical(Exception? exception, string logMessage)
        {
            _logger.LogCritical(exception, logMessage);
            await SendGraylogRequest(exception, logMessage, (int)LogLevel.Critical);
        }

        private async Task SendGraylogRequest(string logMessage, int logLevel)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(logMessage))
                    return;

                var graylogRequest = new GraylogRequest(
                    short_message: logMessage,
                    level: logLevel,
                    serviceName: _grayloggerSettings.ServiceName);

                var httpRequest = new StringContent(JsonConvert.SerializeObject(graylogRequest), Encoding.UTF8, "application/json");
                var (response, statusCode) = await _httpClient.PostAndGetRawResponseAsync(_grayloggerSettings.GraylogUrl, httpRequest);
                _logger.LogInformation($"Graylog API status code: '{statusCode}' and response '{response}'");
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, e.Message);
                throw;
            }
        }

        private async Task SendGraylogRequest(Exception? exception, string? logMessage, int logLevel)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(exception?.Message))
                    return;

                var graylogRequest = new GraylogRequest(
                    short_message: logMessage ?? string.Empty,
                    full_message: exception.Message,
                    level: logLevel,
                    serviceName: _grayloggerSettings.ServiceName);

                var httpRequest = new StringContent(JsonConvert.SerializeObject(graylogRequest), Encoding.UTF8, "application/json");
                var (response, statusCode) = await _httpClient.PostAndGetRawResponseAsync(_grayloggerSettings.GraylogUrl, httpRequest);
                _logger.LogInformation($"Graylog API status code: '{statusCode}' and response '{response}'");
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, e.Message);
                throw;
            }
        }

        public class GraylogRequest
        {
            public GraylogRequest(
                string short_message,
                int level,
                string serviceName)
            {
                this.short_message = short_message;
                this.full_message = string.Empty;
                this.level = level;
                this.serviceName = serviceName;
            }

            public GraylogRequest(
                string short_message,
                string full_message,
                int level,
                string serviceName)
            {
                this.short_message = short_message;
                this.full_message = full_message;
                this.level = level;
                this.serviceName = serviceName;
            }

            public string short_message { get; init; }
            public string full_message { get; init; }
            public int level { get; init; }
            public string serviceName { get; init; }
        }
    }
}
