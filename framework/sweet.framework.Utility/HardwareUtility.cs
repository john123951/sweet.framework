using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Net;
using System.Text;

namespace sweet.framework.Utility
{
    /// <summary>
    /// 得到硬件信息
    /// 添加引用 System.Management
    /// </summary>
    public static class HardwareUtility
    {
        /// <summary>
        /// 获取本地IP地址信息
        /// </summary>
        public static List<string> GetAddressIP4()
        {
            //获取本地的IP地址
            string hostName = Dns.GetHostName();//本机名
            var addressList = Dns.GetHostAddresses(hostName);
            //Dns.GetHostEntry(hostName).AddressList

            //会返回所有地址，包括IPv4和IPv6
            var result = addressList.Where(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                                    .Select(x => x.ToString())
                                    .Distinct()
                                    .ToList();

            return result;
        }

        /// <summary>
        /// 获取本地IP地址信息
        /// </summary>
        public static List<string> GetAddressIP6()
        {
            //获取本地的IP地址
            string hostName = Dns.GetHostName();//本机名
            var addressList = Dns.GetHostAddresses(hostName);

            var result = addressList.Where(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
                                    .Select(x => x.ToString())
                                    .Distinct()
                                    .ToList();

            return result;
        }

        /// <summary>
        /// 返回硬盘序列号标识
        /// </summary>
        /// <returns></returns>
        public static string GetHardDiskSerialNo()
        {
            ManagementClass searcher = new ManagementClass("WIN32_PhysicalMedia");
            ManagementObjectCollection moc = searcher.GetInstances();

            if (moc != null && moc.Count > 0)
            {
                var itor = moc.GetEnumerator();
                if (itor.MoveNext())
                {
                    //硬盘序列号
                    var serialNum = itor.Current["SerialNumber"];

                    if (serialNum != null)
                    {
                        return serialNum.ToString().Trim();
                    }
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// 返回CPU标识
        /// </summary>
        /// <returns></returns>
        public static string GetCpuId()
        {
            ManagementClass searcher = new ManagementClass("WIN32_Processor");
            ManagementObjectCollection moc = searcher.GetInstances();
            var sbCpuIds = new StringBuilder();

            foreach (ManagementObject mo in moc)
            {
                var uniqueId = mo["UniqueId"];
                var processorId = mo["ProcessorId"];
                sbCpuIds.AppendFormat("{0}_{1}_", processorId, uniqueId);
            }
            return sbCpuIds.ToString(0, Math.Max(0, sbCpuIds.Length - 1));
        }
    }
}