using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using sweet.framework.Infrastructure.Cache;
using sweet.framework.Infrastructure.Interfaces;

namespace test.UI.Console.Installers
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