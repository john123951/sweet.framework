using System.Reflection;
using sweet.framework.Utility;

namespace test.UI.Console
{
    public static class BootStrapper
    {
        public static void Configuration()
        {
            ConfigureLog4Net();
            ConfigureDependencies();
            ConfigureMapping();
        }

        private static void ConfigureLog4Net()
        {
            //string xmlPath = HttpContext.Current.Server.MapPath("~/App_Data/Configs/log4net.config");
            //LogUtility.Register(xmlPath);
        }

        private static void ConfigureDependencies()
        {
            WindsorUtility.GetInstance().Register(Assembly.GetExecutingAssembly());
        }

        private static void ConfigureMapping()
        {
            //Mapper.CreateMapper<listdata, ListDataDto>();
        }
    }
}