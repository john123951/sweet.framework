using System;
using System.Collections.Generic;
using sweet.framework.Utility;
using test.UI.Model.Entities;
using test.UI.Service.Contract;

namespace test.UI.Console
{
    internal class MainClass
    {
        public static void Main(string[] args)
        {
            BootStrapper.Configuration();

            var authService = WindsorUtility.GetInstance().Resolve<IAuthService>();
            var userService = WindsorUtility.GetInstance().Resolve<IUserService>();

            {
                var list = authService.GetUserRole(1);
                Print(list);
                authService.SetUserRole(1, new List<RoleInfo>() { new RoleInfo { Id = DateTime.Now.Millisecond, Name = "admin" } });
                var list2 = authService.GetUserRole(1);
                Print(list2);
                var list3 = authService.GetUserRole(1);
                Print(list3);
            }

            {
                var list = userService.GetUserList();
                Print(list);
                userService.InsertUser(new UserInfo { Id = DateTime.Now.Millisecond, UserName = "sweet" });
                var list2 = userService.GetUserList();
                Print(list2);
            }
        }

        private static void Print<T>(IEnumerable<T> itor)
        {
            if (itor == null)
            {
                System.Console.WriteLine("=============================List is Null=============================");
                return;
            }
            System.Console.WriteLine("=============================Print=============================");
            foreach (var item in itor)
            {
                if (itor == null)
                {
                    continue;
                }

                var props = item.GetType().GetProperties();
                foreach (var p in props)
                {
                    System.Console.Write(p.GetValue(item));
                    System.Console.Write("     ");
                }
            }
            System.Console.WriteLine();
            System.Console.WriteLine("===============================================================");

            System.Console.ReadKey();
        }
    }
}