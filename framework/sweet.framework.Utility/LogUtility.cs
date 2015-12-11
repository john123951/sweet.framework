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

        private static LogUtility _instance;

        #region 构造函数

        private LogUtility()
        { }

        static LogUtility()
        {
            LogUtility.Register();
        }

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
            logger.Debug(string.Format(msg, args));
        }

        public void Info(string msg, params object[] args)
        {
            var targetType = new StackFrame(1).GetMethod().DeclaringType;

            var logger = LogManager.GetLogger(targetType.Name);
            logger.Info(string.Format(msg, args));
        }
    }
}