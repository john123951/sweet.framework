using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sweet.framework.Infrastructure.Config
{
    /// <summary>
    /// 读取web.config中的appSetting
    /// </summary>
    public static class AppSettingsConfigManager
    {
        /// <summary>
        /// 列表分页
        /// </summary>
        public static int PageSize
        {
            get
            {
                return ReadConfiguration("WebSite", 10, x => Convert.ToInt32(x));
            }
        }

        /// <summary>
        /// 工行APP传递数据时的密钥
        /// </summary>
        public static string Des3Key
        {
            get
            {
                return ReadConfiguration("des3Key", "WERTYGFDHJAKSJDHCNXBSDFG", x => x);
            }
        }

        /// <summary>
        /// 在*.exe.config文件中appSettings配置节增加一对键、值对
        /// </summary>
        /// <param name="settingKey"></param>
        /// <param name="newValue"></param>
        public static void UpdateAppConfig(string settingKey, string newValue)
        {
            bool isModified = false;
            foreach (string key in ConfigurationManager.AppSettings)
            {
                if (key == settingKey)
                {
                    isModified = true;
                    break;
                }
            }

            // Open App.Config of executable
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            // You need to remove the old settings object before you can replace it
            if (isModified)
            {
                config.AppSettings.Settings.Remove(settingKey);
            }

            // Add an Application Setting.
            config.AppSettings.Settings.Add(settingKey, newValue);

            // Save the changes in App.config file.
            config.Save(ConfigurationSaveMode.Modified);

            // Force a reload of a changed section.
            ConfigurationManager.RefreshSection("appSettings");
        }

        /// <summary>
        /// 读取AppSettings配置节
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="settingKey">AppSettings中的Key值</param>
        /// <param name="defValue">转换失败时的默认值</param>
        /// <param name="converter">转换函数</param>
        /// <returns></returns>
        public static T ReadConfiguration<T>(string settingKey, T defValue, Func<String, T> converter)
        {
            string settingValue = ConfigurationManager.AppSettings[settingKey];

            if (string.IsNullOrWhiteSpace(settingValue))
            {
                return defValue;
            }

            try
            {
                return converter.Invoke(settingValue);
            }
            catch (Exception)
            {
                return defValue;
            }
        }
    }
}