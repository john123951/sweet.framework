using System.ComponentModel;

namespace test.UI.Model.Enums
{
    public enum StatusEnum
    {
        [Description("未知")]
        Unknow = 0,

        [Description("加载中")]
        Loading = 1,

        [Description("加载完成")]
        Completed = 2
    }
}