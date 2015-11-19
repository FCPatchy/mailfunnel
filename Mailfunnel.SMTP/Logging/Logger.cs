using System;

namespace Mailfunnel.SMTP.Logging
{
    public class Logger : ILogger
    {
        public void Log(string info)
        {
            Console.WriteLine($"[{DateTime.Now}] {info}");
        }

        public void LogFormat(string format, params object[] args)
        {
            Log(string.Format(format, args));
        }
    }
}
