using log4net;
using System;

namespace sweet.framework.Utility
{
    public enum Log4NetType
    {
        Debug,
        Info,
        Warn,
        Error,
        Fatal,
    }

    public class LogUtility
    {
        private static LogUtility _instance;

        #region 构造函数

        private LogUtility()
        {
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
            log4net.Config.XmlConfigurator.Configure();
            _instance = new LogUtility();
        }

        public static void Register(string xmlPath)
        {
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new System.IO.FileInfo(xmlPath));
            _instance = new LogUtility();
        }

        #endregion 注册

        public void WriteLog(string logName, Log4NetType type, string message)
        {
            var logger = LogManager.GetLogger(logName);

            Action<string> action = null;
            switch (type)
            {
                case Log4NetType.Debug:
                    action = logger.Debug;
                    break;

                case Log4NetType.Info:
                    action = logger.Info;
                    break;

                case Log4NetType.Warn:
                    action = logger.Warn;
                    break;

                case Log4NetType.Error:
                    action = logger.Error;
                    break;

                case Log4NetType.Fatal:
                    action = logger.Fatal;
                    break;

                default:
                    break;
            }

            if (action != null) { action(message); }
        }

        public void WriteLog(string logName, Log4NetType type, string message, Exception exception)
        {
            var logger = LogManager.GetLogger(logName);

            Action<string, Exception> action = null;
            switch (type)
            {
                case Log4NetType.Debug:
                    action = logger.Debug;
                    break;

                case Log4NetType.Info:
                    action = logger.Info;
                    break;

                case Log4NetType.Warn:
                    action = logger.Warn;
                    break;

                case Log4NetType.Error:
                    action = logger.Error;
                    break;

                case Log4NetType.Fatal:
                    action = logger.Fatal;
                    break;

                default:
                    break;
            }

            if (action != null) { action(message, exception); }
        }
    }
}