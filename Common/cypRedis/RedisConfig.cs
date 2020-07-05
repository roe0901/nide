using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class RedisConfig
    {
		private static RedisConfig _instance;

		public string RedisAddressWrite
		{
			get;
			private set;
		}

		public string RedisAddressRead
		{
			get;
			private set;
		}

		public string CommonRedisAddressWrite
		{
			get;
			private set;
		}

		public string CommonRedisAddressRead
		{
			get;
			private set;
		}

		public int MaxWritePoolSize
		{
			get;
			private set;
		}

		public int MaxReadPoolSize
		{
			get;
			private set;
		}

		public int Timeout
		{
			get;
			private set;
		}

		public static RedisConfig GetInstance()
		{
			if (RedisConfig._instance == null)
			{
				RedisConfig._instance = new RedisConfig();
			}
			return RedisConfig._instance;
		}

		private RedisConfig()
		{
			this.InitRedisConfig();
		}

		public void InitRedisConfig()
		{
			NameValueCollection nameValueCollection = (NameValueCollection)ConfigurationManager.GetSection("RedisSetting");
			this.RedisAddressWrite = nameValueCollection["RedisAddressWrite"];
			this.RedisAddressRead = nameValueCollection["RedisAddressRead"];
			this.CommonRedisAddressWrite = nameValueCollection["CommonRedisAddressWrite"];
			this.CommonRedisAddressRead = nameValueCollection["CommonRedisAddressRead"];
			this.MaxWritePoolSize = Convert.ToInt32(nameValueCollection["MaxWritePoolSize"]);
			this.MaxReadPoolSize = Convert.ToInt32(nameValueCollection["MaxReadPoolSize"]);
			this.Timeout = Convert.ToInt32(nameValueCollection["RedisTimeOut"]);
		}
	}
}
