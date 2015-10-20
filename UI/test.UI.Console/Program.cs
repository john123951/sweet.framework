using sweet.framework.Utility;
using sweet.framework.Utility.Extention;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Dynamic;
using test.UI.Model.Entities;
using test.UI.Service.Contract;

namespace test.UI.Console
{
    internal class MainClass
    {
        public static void Main(string[] args)
        {
            TestGetDescriptionTime();
            BootStrapper.Configuration();

            System.Console.WriteLine(ReflectionUtility.GetCurrentMethodName());

            //TestProductService();
            //TestDynamicLinq();
        }

        private static void TestGetDescriptionTime()
        {
            var status = test.UI.Model.Enums.StatusEnum.Loading;

            var stopWatch = new Stopwatch();
            stopWatch.Start();
            for (int i = 0; i < 100000; i++)
            {
                status.GetDescription();
            }
            stopWatch.Stop();
            System.Console.WriteLine("耗时：{0}ms", stopWatch.ElapsedMilliseconds);

            System.Console.ReadLine();
        }

        private static void TestDynamicLinq()
        {
            var list = new[]
            {
                new {Id = 1, FirstName = "Andrew", Age = 8},
                new {Id = 2, FirstName = "sweet", Age = 7},
                new {Id = 3, FirstName = "winnie", Age = 6},
                new {Id = 4, FirstName = "john", Age = 5},
                new {Id = 5, FirstName = "jack", Age = 4},
                new {Id = 6, FirstName = "lucky", Age = 3},
            };

            //静态查询
            var empList = list.OrderBy(c => c.FirstName).ToList();

            System.Console.WriteLine(empList.Count);

            var findPerson = from employee in empList
                             where employee.FirstName == "Andrew"
                             select employee;

            System.Console.WriteLine(findPerson);

            //动态查询
            var query = list
                .AsQueryable()
                .Where("FirstName == @0 || FirstName == @1", "Andrew", "sweet")
                .Where("Id==@0", 2)
                .Where("FirstName.Length == @0", 5)
                .OrderBy("FirstName")
                .Select("new (FirstName,Age)")
                ;

            System.Console.WriteLine(query.Count());
            System.Console.ReadLine();
        }

        private static void TestProductService()
        {
            var productService = WindsorUtility.GetInstance().Resolve<IProductService>();
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