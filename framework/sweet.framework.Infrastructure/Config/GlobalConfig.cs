namespace sweet.framework.Infrastructure.Config
{
	public static class GlobalConfig
	{
		public static string MemcacheHost{
            get { return "192.168.56.102:11211"; }
		}

		public static string RedisHost{
            get { return "redis://192.168.56.102:6379"; }
		}
	}
}

