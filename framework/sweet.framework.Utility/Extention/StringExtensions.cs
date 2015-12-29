using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/* =======================================================================
* 创建时间：2015/12/11 18:31:07
* 作者：sweet
* Framework: 4.5
* ========================================================================
*/

namespace sweet.framework.Utility.Extention
{
    public static class StringExtensions
    {
        public static IEnumerable<string> EachLines(this string str)
        {
            char[] chars = str.ToCharArray();
            int b = 0;

            for (int i = 0; i < chars.Length - 2; i++)
            {
                if (chars[i] == '\r' || chars[i + 1] == '\n')
                {
                    yield return new string(chars, b, i - b);
                    b = i + 2;
                }
            }
        }
    }
}