namespace Mailfunnel.SMTP.Logging
{
    public interface ILogger
    {
        void Log(string info);
        void LogFormat(string format, params object[] args);
    }
}
