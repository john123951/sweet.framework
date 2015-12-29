using System.Diagnostics;

namespace sweet.framework.Utility
{
    public static class DebugUtility
    {
        /// <summary>
        /// 输出到命令行
        /// </summary>
        public static void SetConsoleOutput()
        {
            Debug.Listeners.Add(new ConsoleTraceListener());
            //Debug.Listeners.Add(new TextWriterTraceListener(Console.Out));
        }

        /// <summary>
        /// 输出到web调试
        /// 访问 /trace.axd
        /// </summary>
        public static void SetWebTraceOutput()
        {
            //Debug.Listeners.Add(new WebPageTraceListener());
        }
    }
}