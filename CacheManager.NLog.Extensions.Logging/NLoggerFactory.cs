using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLog;
using CacheManager.Core.Logging;
using CacheManager.Core.Utility;
using System.Globalization;
using LogLevel = CacheManager.Core.Logging.LogLevel;
using NlogLogLevel = NLog.LogLevel;
using NLogILogger = NLog.ILogger;

namespace CacheManager.Logging
{
    public class NLoggerFactoryAdapter: ILoggerFactory, IDisposable
    {
        public NLoggerFactoryAdapter()
        {
        }

        //public NLoggerFactoryAdapter(ILoggerFactory parentFactory)
        //{
        //    Guard.NotNull(parentFactory, nameof(parentFactory));
        //    this.parentFactory = parentFactory;
        //}
        public Core.Logging.ILogger CreateLogger(string categoryName)
        {
            var logger = LogManager.GetLogger(categoryName);
            return new NLoggerAdapter(logger);
        }

        public Core.Logging.ILogger CreateLogger<T>(T instance)
        {
            var logger = LogManager.GetLogger(nameof(instance), instance.GetType());
            return new NLoggerAdapter(logger);
        }

        public void Dispose()
        {
            this.Dispose();
            GC.SuppressFinalize(this);
        }
    }

    internal class NLoggerAdapter : Core.Logging.ILogger
    {
        
        private static readonly Func<object, Exception, string> Formatter = MessageFormatter;
        
        private readonly NLogILogger logger;
        
        public NLoggerAdapter(NLogILogger logger)
        {
            Guard.NotNull(logger, nameof(logger));

            this.logger = logger;
        }

        public IDisposable BeginScope(object state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return this.logger.IsEnabled(GetExternalLogLevel(logLevel));
        }

        public void Log(LogLevel logLevel, int eventId, object message, Exception exception)
        {
            //this.logger.Log(GetExternalLogLevel(logLevel), eventId, message, exception, Formatter);
            this.logger.Log(GetExternalLogLevel(logLevel), exception, message.ToString(), eventId);
        }

        private static string MessageFormatter(object state, Exception error)
        {
            if (state == null && error == null)
            {
                throw new InvalidOperationException("No message or exception details were found to create a message for the log.");
            }

            if (state == null)
            {
                return error.ToString();
            }

            if (error == null)
            {
                return state.ToString();
            }

            return string.Format(CultureInfo.CurrentCulture, "{0}{1}{2}", state, Environment.NewLine, error);
        }

        private static NlogLogLevel GetExternalLogLevel(LogLevel level)
        {
            switch (level)
            {
                case LogLevel.Debug:
                    return NlogLogLevel.Debug;
                case LogLevel.Trace:
                    return NlogLogLevel.Trace;
                case LogLevel.Information:
                    return NlogLogLevel.Info;
                case LogLevel.Warning:
                    return NlogLogLevel.Warn;
                case LogLevel.Error:
                    return NlogLogLevel.Error;
                case LogLevel.Critical:
                    return NlogLogLevel.Fatal;
            }

            return NlogLogLevel.Off;
        }
    }
}
