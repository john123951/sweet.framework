using System;
using System.Linq;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.MicroKernel.SubSystems.Configuration;
using test.Infrastructure;
using Castle.Windsor.Installer;
using Castle.DynamicProxy;
using test.Interceptors;

namespace test.ConsoleUI
{
	public class InterceptorInstaller : IWindsorInstaller
	{
		public void Install (IWindsorContainer container, IConfigurationStore store)
		{
			var asm = typeof(TraceInterceptor).Assembly;

			container.Register (Classes.FromAssembly (asm).BasedOn<IInterceptor> ());
		}
	}
}

