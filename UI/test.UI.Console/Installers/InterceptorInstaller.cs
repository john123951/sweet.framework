using Castle.DynamicProxy;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using sweet.framework.Infrastructure.Interceptors;

namespace test.UI.Console.Installers
{
    public class InterceptorInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Classes.FromAssemblyContaining<CacheInterceptor>()   //.FromAssemblyInThisApplication()
                                      .BasedOn<IInterceptor>()
                                      .LifestyleSingleton()
                                      );
        }
    }
}