using System;
using System.ComponentModel;
using System.Reflection;

namespace sweet.framework.Utility.Extention
{
    /// <summary>
    /// 枚举类型扩展方法类
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// 获取枚举值的详细文本[Description]
        /// </summary>
        /// <param name="targetEnum"></param>
        /// <returns></returns>
        public static string GetDescription(this System.Enum targetEnum)
        {
            if (targetEnum == null) { return string.Empty; }

            Type type = targetEnum.GetType();
            string strTarget = Enum.GetName(type, targetEnum); //target.ToString();

            //获取字段信息
            System.Reflection.FieldInfo[] arrFieldInfo = type.GetFields();

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

                //反射出自定义属性

                //var arrAttr = fieldInfo.GetCustomAttributes(true);
                //for (int j = arrAttr.Length - 1; j >= 0; j--)
                //{
                //    var attr = arrAttr[j];
                //    //类型转换找到一个Description，用Description作为成员名称
                //    var dscript = attr as DescriptionAttribute;
                //    if (dscript != null)
                //    {
                //        return dscript.Description;
                //    }
                //}

                #endregion 3.5
            }

            //如果没有检测到合适的注释，则用默认名称
            return strTarget;
        }
    }
}