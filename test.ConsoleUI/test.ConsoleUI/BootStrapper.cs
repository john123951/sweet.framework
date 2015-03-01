using System;
using Castle.Windsor;
using Castle.Windsor.Installer;

namespace test.ConsoleUI
{
	public static class BootStrapper
	{
		public static readonly WindsorContainer container = new WindsorContainer ();

		public static void Configuration ()
		{
			container.Install (FromAssembly.This ());
		}
	}
}

