using DapperExtensions.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/* =======================================================================
* 创建时间：2015/12/31 16:15:53
* 作者：sweet
* Framework: 4.0
* ========================================================================
*/

namespace test.UI.Respository.Mapper
{
    public class EntityTableMapper<T> : AutoClassMapper<T>
        where T : class
    {
        public EntityTableMapper()
        {
            string endFix = "Entity";

            Type type = typeof(T);
            string tableName = type.Name;

            if (type.Name.EndsWith(endFix, StringComparison.Ordinal))
            {
                tableName = tableName.Substring(0, tableName.Length - endFix.Length);
            }

            Table(tableName);
            AutoMap();
        }
    }
}