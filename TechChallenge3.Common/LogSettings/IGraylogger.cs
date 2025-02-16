namespace TechChallenge3.Common.LogSettings
{
    public interface IGraylogger
    {
        Task LogDebug(string logMessage);
        Task LogDebug(Exception? exception, string? logMessage);
        Task LogInformation(string logMessage);
        Task LogInformation(Exception? exception, string logMessage);
        Task LogWarning(string logMessage);
        Task LogWarning(Exception? exception, string logMessage);
        Task LogError(string logMessage);
        Task LogError(Exception? exception, string logMessage);
        Task LogCritical(string logMessage);
        Task LogCritical(Exception? exception, string logMessage);
    }
}
