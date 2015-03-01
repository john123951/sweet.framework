using System;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.MicroKernel.SubSystems.Configuration;
using test.Infrastructure;
using test.Interceptors;
using test.Service;

namespace test.ConsoleUI
{
	public class ServiceInstaller : IWindsorInstaller
	{
		public void Install (IWindsorContainer container, IConfigurationStore store)
		{
//			container.Register (Component.For<IAuthService> ().ImplementedBy<AuthService> ());
//			container.Register (Component.For<IProductService> ().ImplementedBy<ProductService> ());
//			container.Register (Component.For<IUserService> ().ImplementedBy<UserService> ());

			var asm = typeof(AuthService).Assembly;

			container.Register (Classes.FromAssembly (asm)
				.BasedOn<IMainService> ()
				.WithServiceDefaultInterfaces ()
				.Configure (x => x
					.LifestyleSingleton ()
					.Interceptors (new [] { typeof(LogInterceptor), typeof(TraceInterceptor), typeof(CacheInterceptor) })
			));

		}
	}
}
