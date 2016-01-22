using Pinyin4net;
using sweet.framework.Utility.Extention;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/* =======================================================================
* 创建时间：2016/1/21 13:46:57
* 作者：sweet
* Framework: 4.5
* ========================================================================
*/

namespace sweet.framework.Utility
{
    public static class PinyinUtility
    {
        public static List<string> GetShortPinyin(string input)
        {
            List<List<char>> allPY = new List<List<char>>();

            foreach (var item in input)
            {
                var arrPinyin = PinyinHelper.ToHanyuPinyinStringArray(item);

                if (!arrPinyin.IsNotEmpty()) { continue; }

                //取出多音字中所有第一个字母
                var fchars = arrPinyin
                                 .Where(x => x != null)
                                 .Select(x => x.FirstOrDefault())
                                 .Distinct()
                                 .ToList();

                //将此集合加入到 allPY
                allPY.Add(fchars);
            }
            //遍历 allPY ，生成可能组成的简拼集合
            List<string> result = new List<string>();
            CreateCASE(allPY, ref result);

            return result.Distinct().ToList();
        }

        #region 姓名转拼音

        public static string GetXingmingPinyin(string name)
        {
            if (_xingmingDict == null || !_xingmingDict.Any())
            {
                lock (_lock)
                {
                    if (_xingmingDict == null || !_xingmingDict.Any())
                    {
                        _xingmingDict = new Dictionary<string, string>();

                        using (var stream = ResourceUtility.ReadStream("Resources.xingmingpinyin.txt"))
                        using (var reader = new StreamReader(stream, Encoding.UTF8))
                        {
                            //skip
                            reader.ReadLine();

                            while (!reader.EndOfStream)
                            {
                                string line = reader.ReadLine();
                                var arr = line.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                                if (arr != null && arr.Length == 3)
                                {
                                    _xingmingDict[arr[1]] = arr[2];
                                }
                            }
                        }
                    }
                }
            }
            if (string.IsNullOrWhiteSpace(name)) { return string.Empty; }
            name = name.Trim();

            var sbResult = new StringBuilder();
            foreach (char item in name)
            {
                string key = item.ToString();

                if (_xingmingDict.ContainsKey(key))
                {
                    sbResult.Append(_xingmingDict[key]);
                }
            }

            return sbResult.ToString();
        }

        private static object _lock = new object();
        private static Dictionary<string, string> _xingmingDict = null;

        #endregion 姓名转拼音

        #region 私有方法

        /// <summary>
        /// 生成可能组成的简拼集合
        /// </summary>
        /// <param name="inputList">需要处理的集合</param>
        /// <param name="result">接收处理结果的集合</param>
        /// <param name="startIndex">从第几个字开始处理</param>
        /// <param name="path">拼接字符串用的，无需传参</param>
        private static void CreateCASE(List<List<char>> inputList, ref List<string> result, int startIndex = 0, Stack<char> path = null)
        {
            if (path == null) { path = new Stack<char>(); }
            if (result == null) { result = new List<string>(); }

            if (startIndex >= inputList.Count) { return; }

            //找到这个字
            var charPys = inputList[startIndex];

            //遍历此字的所有音
            for (int k = 0; k < charPys.Count; ++k)
            {
                //把这个音加到拼接字符串中
                path.Push(charPys[k]);

                if (startIndex == inputList.Count - 1)
                {
                    //是最后一个字
                    result.Add(new string(path.Reverse().ToArray()));
                }
                else
                {
                    //不是最后一个字，递归下一个字
                    CreateCASE(inputList, ref result, startIndex + 1, path);
                }
                path.Pop();
            }
        }

        #endregion 私有方法
    }
}