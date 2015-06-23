using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using sweet.framework.Infrastructure.Interceptors;
using sweet.framework.Infrastructure.Interfaces;
using test.UI.Service;

namespace test.UI.Console.Installers
{
    public class ServiceInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            //			container.Register (Component.For<IAuthService> ().ImplementedBy<AuthService> ());
            //			container.Register (Component.For<IProductService> ().ImplementedBy<ProductService> ());
            //			container.Register (Component.For<IUserService> ().ImplementedBy<UserService> ());

            var asm = typeof(AuthService).Assembly;

            container.Register(Classes.FromAssembly(asm)
                .BasedOn<IService>()
                .WithServiceDefaultInterfaces()
                .Configure(x => x
                    .LifestyleSingleton()
                    .Interceptors(new[] { typeof(TryCatchInterceptor), typeof(TraceInterceptor), typeof(CacheInterceptor) })
            ));
        }
    }
}