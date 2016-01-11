using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

/* =======================================================================
* 创建时间：2016/1/11 10:41:24
* 作者：sweet
* Framework: 4.5
* ========================================================================
*/

namespace sweet.framework.Infrastructure.Config
{
    /// <summary>
    /// 修改默认的app.config文件名
    /// Usage is like this:
    ///
    ///     // the default app.config is used.
    ///     using(AppConfig.Change(tempFileName))
    ///     {
    ///     // the app.config in tempFileName is used
    ///     }
    ///     // the default app.config is used.
    ///
    /// </summary>
    public abstract class AppConfig : IDisposable
    {
        public static AppConfig Change(string path)
        {
            return new ChangeAppConfig(path);
        }

        public abstract void Dispose();

        private class ChangeAppConfig : AppConfig
        {
            private readonly string oldConfig = AppDomain.CurrentDomain.GetData("APP_CONFIG_FILE").ToString();

            private bool disposedValue;

            public ChangeAppConfig(string path)
            {
                AppDomain.CurrentDomain.SetData("APP_CONFIG_FILE", path);
                ResetConfigMechanism();
            }

            public override void Dispose()
            {
                if (!disposedValue)
                {
                    AppDomain.CurrentDomain.SetData("APP_CONFIG_FILE", oldConfig);
                    ResetConfigMechanism();

                    disposedValue = true;
                }
                GC.SuppressFinalize(this);
            }

            private static void ResetConfigMechanism()
            {
                typeof(ConfigurationManager)
                    .GetField("s_initState", BindingFlags.NonPublic |
                                             BindingFlags.Static)
                    .SetValue(null, 0);

                typeof(ConfigurationManager)
                    .GetField("s_configSystem", BindingFlags.NonPublic |
                                                BindingFlags.Static)
                    .SetValue(null, null);

                typeof(ConfigurationManager)
                    .Assembly.GetTypes()
                    .Where(x => x.FullName ==
                                "System.Configuration.ClientConfigPaths")
                    .First()
                    .GetField("s_current", BindingFlags.NonPublic |
                                           BindingFlags.Static)
                    .SetValue(null, null);
            }
        }
    }
}