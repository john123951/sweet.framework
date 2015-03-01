using System;

namespace test.Utility
{
	public static class ConsoleUtility
	{
		public static void WriteLine (string msg, ConsoleColor color)
		{
			WriteLine (msg, color, null);
		}

		public static void WriteLine (string msg, ConsoleColor color, params object[] arg)
		{
			Console.ForegroundColor = color;
			Console.WriteLine (msg, arg);
			Console.ResetColor ();
		}
	}
}

