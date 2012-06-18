using System;
using log4net;
using ILog = Acid.PuntoPagos.Sdk.Interfaces.ILog;

namespace Acid.PuntoPagos.Sdk.Imp
{
    internal sealed class Log4NetLoggerProxy : ILog
    {
        private readonly log4net.ILog _netLog;

        private Log4NetLoggerProxy(log4net.ILog log)
        {
            _netLog = log;
        }

        #region ILog Members

        public void Debug(object message)
        {
            _netLog.Debug(message);
        }

        public void Info(object message)
        {
            _netLog.Info(message);
        }

        public void Warn(object message)
        {
            _netLog.Warn(message);
        }

        public void Error(object message)
        {
            _netLog.Error(message);
        }

        public void Fatal(object message)
        {
            _netLog.Fatal(message);
        }

        public void Debug(object message, Exception t)
        {
            _netLog.Debug(message, t);
        }

        public void Info(object message, Exception t)
        {
            _netLog.Info(message, t);
        }

        public void Warn(object message, Exception t)
        {
            _netLog.Warn(message, t);
        }

        public void Error(object message, Exception t)
        {
            _netLog.Error(message, t);
        }

        public void Fatal(object message, Exception t)
        {
            _netLog.Fatal(message, t);
        }

        public void DebugFormat(string format, params object[] args)
        {
            _netLog.DebugFormat(format, args);
        }

        public void InfoFormat(string format, params object[] args)
        {
            _netLog.InfoFormat(format, args);
        }

        public void WarnFormat(string format, params object[] args)
        {
            _netLog.FatalFormat(format, args);
        }

        public void ErrorFormat(string format, params object[] args)
        {
            _netLog.ErrorFormat(format, args);
        }

        public void FatalFormat(string format, params object[] args)
        {
            _netLog.FatalFormat(format, args);
        }

        #endregion

        public static ILog GetLogger(string loggerType)
        {
            return new Log4NetLoggerProxy(LogManager.GetLogger(loggerType));
        }
    }
}