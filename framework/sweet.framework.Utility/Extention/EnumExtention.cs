using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace sweet.framework.Utility.Extention
{
    /// <summary>
    /// 枚举类型扩展方法类
    /// </summary>
    public static class EnumExtention
    {
        private static readonly Dictionary<RuntimeTypeHandle, FieldInfo[]> CacheFieldInfo = new Dictionary<RuntimeTypeHandle, FieldInfo[]>();

        /// <summary>
        /// 获取枚举值的详细文本[Description]
        /// </summary>
        /// <param name="targetEnum"></param>
        /// <returns></returns>
        public static string GetDescription(this System.Enum targetEnum)
        {
            if (targetEnum == null) { return string.Empty; }

            Type t = targetEnum.GetType();
            var typeHandle = t.TypeHandle;
            string strTarget = Enum.GetName(t, targetEnum); //target.ToString();

            //获取字段信息
            System.Reflection.FieldInfo[] arrFieldInfo;
            if (CacheFieldInfo.ContainsKey(typeHandle))
            {
                arrFieldInfo = CacheFieldInfo[typeHandle];
            }
            else
            {
                arrFieldInfo = t.GetFields();
                CacheFieldInfo[typeHandle] = arrFieldInfo;
            }

            for (int i = arrFieldInfo.Length - 1; i >= 0; i--)
            {
                var fieldInfo = arrFieldInfo[i];

                //判断名称是否相等
                if (fieldInfo.Name != strTarget) continue;

                #region 4.5

                //反射出自定义属性
                var dscript = fieldInfo.GetCustomAttribute<DescriptionAttribute>(true);

                //类型转换找到一个Description，用Description作为成员名称
                if (dscript != null) { return dscript.Description; }

                #endregion 4.5

                #region 3.5

                ////反射出自定义属性
                //if (CacheDescriptionAttr.ContainsKey(strTarget))
                //{
                //    var dscript = CacheDescriptionAttr[strTarget];
                //    return dscript.Description;
                //}
                //else
                //{
                //    var arrAttr = fieldInfo.GetCustomAttributes(true);
                //    for (int j = arrAttr.Length - 1; j >= 0; j--)
                //    {
                //        var attr = arrAttr[j];
                //        //类型转换找到一个Description，用Description作为成员名称
                //        var dscript = attr as DescriptionAttribute;
                //        if (dscript != null)
                //        {
                //            CacheDescriptionAttr[strTarget] = dscript;
                //            return dscript.Description;
                //        }
                //    }
                //}

                #endregion 3.5
            }

            //如果没有检测到合适的注释，则用默认名称
            return strTarget;
        }
    }
}