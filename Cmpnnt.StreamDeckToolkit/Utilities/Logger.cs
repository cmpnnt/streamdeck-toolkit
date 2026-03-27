using System;
using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace Cmpnnt.StreamDeckToolkit.Utilities
{
    /// <summary>
    /// Tracing levels used for Logger
    /// </summary>
    public enum TracingLevel
    {
        /// <summary>
        /// Represents messages that should only be used for debugging
        /// </summary>
        Debug,
        /// <summary>
        /// Represents informational messages
        /// </summary>
        Info,
        /// <summary>
        /// Represents warnings that might need attention
        /// </summary>
        Warn,
        /// <summary>
        /// Represents errors that need attention and might cause issues
        /// </summary>
        Error,
        /// <summary>
        /// Represents fatal errors
        /// </summary>
        Fatal
    }

    /// <summary>
    /// Logger helper class
    /// </summary>
    public class Logger
    {
        private static readonly Lazy<Logger> instance = new(() => new Logger());
        private readonly LoggingLevelSwitch levelSwitch = new(LogEventLevel.Warning);

        /// <summary>
        /// Returns singleton instance of logger
        /// </summary>
        public static Logger Instance => instance.Value;

        private readonly Serilog.Core.Logger log;

        private Logger()
        {
            log = new LoggerConfiguration()
                .MinimumLevel.ControlledBy(levelSwitch)
                .Enrich.WithThreadId()
                .WriteTo.File(
                    "pluginlog_.log",
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 4,
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.ffff}|{Level:u}|{ThreadId}|{Message:lj}{NewLine}{Exception}")
                .CreateLogger();
            LogMessage(TracingLevel.Warn, "Logger Initialized");
        }

        /// <summary>
        /// Add message to log with a specific severity level.
        /// </summary>
        /// <param name="level"></param>
        /// <param name="message"></param>
        public void LogMessage(TracingLevel level, string message)
        {
            switch (level)
            {
                case TracingLevel.Debug:
                    log.Debug("{Message}", message);
                    break;

                case TracingLevel.Info:
                    log.Information("{Message}", message);
                    break;

                case TracingLevel.Warn:
                    log.Warning("{Message}", message);
                    break;

                case TracingLevel.Error:
                    log.Error("{Message}", message);
                    break;

                case TracingLevel.Fatal:
                    log.Fatal("{Message}", message);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(level), level, null);
            }
        }

        public void SetVerbose(bool verbose)
        {
            levelSwitch.MinimumLevel = verbose ? LogEventLevel.Debug : LogEventLevel.Warning;
        }
    }
}
