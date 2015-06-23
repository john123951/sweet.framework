using System.Reflection;
using test.Utility;
using test.Utility.Serialization;

namespace test.ConsoleUI
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