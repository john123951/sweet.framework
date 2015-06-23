using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using test.Infrastructure.Cache;
using test.Infrastructure.Interfaces;

namespace test.ConsoleUI.Installers
{
    public class CacheInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<ICacheProvider>()
                                        //.ImplementedBy<MemcachedCacheProvider> ()
                                        //.ImplementedBy<RedisCacheProvider>()
                                        .ImplementedBy<NLiteCacheProvider>()
                                        .LifestyleSingleton()
                //						.Interceptors (new []{ typeof(TraceInterceptor) })
            );
        }
    }
}