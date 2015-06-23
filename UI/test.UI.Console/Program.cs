using sweet.framework.Utility;
using System;
using System.Collections.Generic;
using test.UI.Model.Entities;
using test.UI.Service.Contract;

namespace test.UI.Console
{
    internal class MainClass
    {
        public static void Main(string[] args)
        {
            BootStrapper.Configuration();

            var productService = WindsorUtility.GetInstance().Resolve<IProductService>();

            {
                int total;
                const long userId = 112;

                System.Console.WriteLine("GetProductList");
                var list = productService.GetProductList(userId, 0, 5, out total);
                Print(list);

                System.Console.WriteLine("InsertUser");
                productService.AddProduct(new ProductInfo()
                {
                    Id = DateTime.Now.Millisecond,
                    Name = "Book",
                    Price = 100,
                    UserId = userId
                });

                System.Console.WriteLine("GetProductList");
                var list2 = productService.GetProductList(userId, 0, 5, out total);
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