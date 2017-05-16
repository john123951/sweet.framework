using log4net;
using System;
using System.Diagnostics;

namespace sweet.framework.Utility
{
    public class LogUtility
    {
        public enum LogType
        {
            Debug,
            Info,
            Warn,
            Error,
            Fatal,
        }

        #region 构造函数

        private static LogUtility _instance;

        private LogUtility()
        { }

        public static LogUtility GetInstance()
        {
            if (_instance == null)
            {
                throw new Exception("Must Call Register() Method");
            }
            return _instance;
        }

        #endregion 构造函数

        #region 注册

        static LogUtility()
        {
            LogUtility.Register();
        }

        public static void Register()
        {
            log4net.Config.BasicConfigurator.Configure();
            _instance = new LogUtility();
        }

        public static void Register(string xmlPath)
        {
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new System.IO.FileInfo(xmlPath));
            _instance = new LogUtility();
        }

        #endregion 注册

        public static void TraceOutput()
        {
            System.Diagnostics.Debug.Listeners.Add(new Log4netTraceListener());
            System.Diagnostics.Trace.Listeners.Add(new Log4netTraceListener());
        }

        public void WriteLog(string logName, LogType type, string message)
        {
            var logger = LogManager.GetLogger(logName);

            Action<string> action = null;
            switch (type)
            {
                case LogType.Debug:
                    action = logger.Debug;
                    break;

                case LogType.Info:
                    action = logger.Info;
                    break;

                case LogType.Warn:
                    action = logger.Warn;
                    break;

                case LogType.Error:
                    action = logger.Error;
                    break;

                case LogType.Fatal:
                    action = logger.Fatal;
                    break;

                default:
                    break;
            }

            if (action != null) { action(message); }
        }

        public void WriteLog(string logName, LogType type, string message, Exception exception)
        {
            var logger = LogManager.GetLogger(logName);

            Action<string, Exception> action = null;
            switch (type)
            {
                case LogType.Debug:
                    action = logger.Debug;
                    break;

                case LogType.Info:
                    action = logger.Info;
                    break;

                case LogType.Warn:
                    action = logger.Warn;
                    break;

                case LogType.Error:
                    action = logger.Error;
                    break;

                case LogType.Fatal:
                    action = logger.Fatal;
                    break;

                default:
                    break;
            }

            if (action != null) { action(message, exception); }
        }

        public void Debug(string msg, params object[] args)
        {
            var targetType = new StackFrame(1).GetMethod().DeclaringType;
            var logger = LogManager.GetLogger(targetType.Name);

            if (args == null || args.Length <= 0)
            {
                logger.Debug(msg);
            }
            else
            {
                logger.DebugFormat(msg, args);
            }
        }

        public void Info(string msg, params object[] args)
        {
            var targetType = new StackFrame(1).GetMethod().DeclaringType;
            var logger = LogManager.GetLogger(targetType.Name);

            if (args == null || args.Length <= 0)
            {
                logger.Info(msg);
            }
            else
            {
                logger.InfoFormat(msg, args);
            }
        }

        public void Warn(string msg, params object[] args)
        {
            var targetType = new StackFrame(1).GetMethod().DeclaringType;
            var logger = LogManager.GetLogger(targetType.Name);

            if (args == null || args.Length <= 0)
            {
                logger.Warn(msg);
            }
            else
            {
                logger.WarnFormat(msg, args);
            }
        }

        public void Error(string msg, params object[] args)
        {
            var targetType = new StackFrame(1).GetMethod().DeclaringType;
            var logger = LogManager.GetLogger(targetType.Name);

            if (args == null || args.Length <= 0)
            {
                logger.Error(msg);
            }
            else
            {
                logger.ErrorFormat(msg, args);
            }
        }

        public void Fatal(string msg, params object[] args)
        {
            var targetType = new StackFrame(1).GetMethod().DeclaringType;
            var logger = LogManager.GetLogger(targetType.Name);

            if (args == null || args.Length <= 0)
            {
                logger.Fatal(msg);
            }
            else
            {
                logger.FatalFormat(msg, args);
            }
        }

        #region 内部类

        public class Log4netTraceListener : System.Diagnostics.TraceListener
        {
            private readonly log4net.ILog _log;

            public Log4netTraceListener()
            {
                _log = log4net.LogManager.GetLogger("System.Diagnostics.Redirection");
            }

            public Log4netTraceListener(log4net.ILog log)
            {
                _log = log;
            }

            public override void Write(string message)
            {
                if (_log != null)
                {
                    _log.Debug(message);
                }
            }

            public override void WriteLine(string message)
            {
                if (_log != null)
                {
                    _log.Debug(message);
                }
            }
        }

        #endregion 内部类
    }
}