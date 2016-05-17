using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using sweet.framework.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using test.UI.Respository;
using test.UI.Respository.Contract;
using test.UI.Respository.Entities;

/* =======================================================================
* 创建时间：2016/5/17 13:20:24
* 作者：sweet
* Framework: 4.5
* ========================================================================
*/

namespace test.UI.Console.Installers
{
    public class RepositoryInstall : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            //string mysqlConnection = ConfigurationManager.ConnectionStrings["mysql_insurance"].ConnectionString;

            //container.Register(Component.For(typeof(IRepository<,>))
            //                .ImplementedBy(typeof(Linq2DbRepository<>))
            //                .DependsOn(Dependency.OnConfigValue("connectionString", mysqlConnection))
            //                .LifestyleSingleton()
            //                );

            container.Register(Component.For(typeof(IRepository<,>))
                                        .ImplementedBy(typeof(MemoryRepository<,>))
                                        .LifestyleSingleton()
                                        );

            container.Register(Component.For(typeof(IRoleRepository))
                                        .ImplementedBy(typeof(MemoryRepository<RoleEntity, long>))
                                        .LifestyleSingleton()
                                        );
            container.Register(Component.For(typeof(IProductRepository))
                                        .ImplementedBy(typeof(MemoryRepository<ProductEntity, long>))
                                        .LifestyleSingleton()
                                        );
        }
    }
}