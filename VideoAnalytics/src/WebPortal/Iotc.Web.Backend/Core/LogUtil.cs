using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;

namespace Iotc.Web.Backend.Models
{
    public enum LogLevel
    {
        Info,
        Warning,
        Error
    }

    public class LogUtil
    {
        public static void Log(string message, LogLevel level = LogLevel.Info, [CallerMemberName] string callerName = "")
        {
            try
            {
             #if DEBUG
            Debug.WriteLine("[{0}][{1}]{2}: {3}", DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff"), level, callerName, message);
            #endif
            }
            catch (Exception)
            {
                // ignored
            }
        }

        public static void LogException(Exception e, string additional = "", [CallerMemberName] string callerName = "")
        {
            try
            {
                var sb = new StringBuilder();
                sb.AppendLine("Exception:");

                var exception = e;
                while (exception.InnerException != null)
                {
                    sb.AppendLine(exception.Message);
                    exception = exception.InnerException;
                }

                sb.AppendLine(e.Message);

                if (!string.IsNullOrEmpty(additional))
                {
                    sb.AppendLine($"Additional message: {additional}.");
                }

                #if DEBUG
                Debug.WriteLine("[{0}][{1}]{2}: {3}", DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff"), LogLevel.Error, callerName, sb);
                #endif
            }
            catch (Exception)
            {
                // ignore
            }
        }
    }
}
