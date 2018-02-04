using System;

namespace MunicipalityTaxes.Core.Logging
{
    public class NullLogger : ILogger
    {
        public NullLogger(Type type)
        {
        }

        void ILogger.Debug(object message)
        {
        }

        void ILogger.Debug(object message, Exception exception)
        {
        }

        void ILogger.DebugFormat(string format, params object[] args)
        {
        }

        void ILogger.Error(object message)
        {
        }

        void ILogger.Error(object message, Exception exception)
        {
        }

        void ILogger.ErrorFormat(string format, params object[] args)
        {
        }

        void ILogger.Fatal(object message)
        {
        }

        void ILogger.Fatal(object message, Exception exception)
        {
        }

        void ILogger.FatalFormat(string format, params object[] args)
        {
        }

        void ILogger.Info(object message)
        {
        }

        void ILogger.Info(object message, Exception exception)
        {
        }

        void ILogger.InfoFormat(string format, params object[] args)
        {
        }

        void ILogger.Warn(object message)
        {
        }

        void ILogger.Warn(object message, Exception exception)
        {
        }

        void ILogger.WarnFormat(string format, params object[] args)
        {
        }
    }
}
