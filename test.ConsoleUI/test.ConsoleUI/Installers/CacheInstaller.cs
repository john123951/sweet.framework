using System;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.MicroKernel.SubSystems.Configuration;
using test.Infrastructure;
using test.Interceptors;

namespace test.ConsoleUI
{
	public class CacheInstaller : IWindsorInstaller
	{
		public void Install (IWindsorContainer container, IConfigurationStore store)
		{
			container.Register (Component.For<ICacheProvider> ()
				.ImplementedBy<MemcachedCacheProvider> ()
				.LifestyleSingleton ()
				//.Interceptors (new []{ typeof(TraceInterceptor) })
			);
		}
	}
}

