namespace test.Infrastructure.Config
{
	public static class GlobalConfig
	{
		public static string MemcacheHost{ 
			get { return "127.0.0.1:11211"; }
		}

		public static string RedisHost{ 
			get { return "redis://localhost:6379"; }
		}
	}
}

