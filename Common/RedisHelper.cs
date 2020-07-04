using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class RedisHelper
    {

		private static readonly RedisClientManagerConfig redisCfg = new RedisClientManagerConfig
		{
			AutoStart = true,
			MaxWritePoolSize = RedisConfig.GetInstance().MaxReadPoolSize,
			MaxReadPoolSize = RedisConfig.GetInstance().MaxReadPoolSize
		};

		private PooledRedisClientManager _clientsManager;

		public static RedisHelper GetInstance
		{
			get
			{
				return RedisHelper.LoadPoolRedis(RedisConfig.GetInstance().RedisAddressWrite, RedisConfig.GetInstance().RedisAddressRead);
			}
		}

		public static RedisHelper GetCommonInstance
		{
			get
			{
				return RedisHelper.LoadPoolRedis(RedisConfig.GetInstance().CommonRedisAddressWrite, RedisConfig.GetInstance().CommonRedisAddressRead);
			}
		}

		public RedisHelper(PooledRedisClientManager clientsManager)
		{
			this._clientsManager = clientsManager;
		}

		private static RedisHelper LoadPoolRedis(string addressWrite, string addressRead)
		{
			
			PooledRedisClientManager clientsManager = new PooledRedisClientManager(addressWrite.Split(new char[]
			{
				','
			}), addressRead.Split(new char[]
			{
				','
			}), RedisHelper.redisCfg)
			{
				ConnectTimeout = new int?(RedisConfig.GetInstance().Timeout)
			};
			return new RedisHelper(clientsManager);
		}

		public RedisClient GetRedisClient()
		{
			return this._clientsManager.GetClient() as RedisClient;
		}

		public RedisClient GetRedisReadClient()
		{
			return this._clientsManager.GetReadOnlyClient() as RedisClient;
		}

		public bool ExpireEntryIn(string key, TimeSpan ts)
		{
			bool result;
			using (RedisClient redisClient = this.GetRedisClient())
			{
				result = redisClient.ExpireEntryIn(key, ts);
			}
			return result;
		}

		public void EnquequeItem(string key, string value)
		{
			using (RedisClient redisClient = this.GetRedisClient())
			{
				redisClient.EnqueueItemOnList(key, value);
			}
		}

		public string DequeueItem(string key)
		{
			string result;
			using (RedisClient redisClient = this.GetRedisClient())
			{
				result = redisClient.DequeueItemFromList(key);
			}
			return result;
		}

		public List<string> GetRangeFromList(string key)
		{
			Stopwatch stopwatch = Stopwatch.StartNew();
			List<string> result;
			using (RedisClient redisClient = this.GetRedisClient())
			{
				List<string> list = new List<string>();
				int num = (int)redisClient.GetListCount(key);
				if (num == 0)
				{
					result = list;
				}
				else
				{
					int num2 = (num > 100) ? 100 : num;
					int startingFrom = num - num2;
					list = redisClient.GetRangeFromList(key, startingFrom, num);
					list.Reverse();
					foreach (string current in list)
					{
						redisClient.RemoveItemFromList(key, current);
					}
					stopwatch.Stop();
					result = list;
				}
			}
			return result;
		}

		public bool IsContainsKey(string key)
		{
			bool result;
			using (RedisClient redisClient = this.GetRedisClient())
			{
				result = redisClient.ContainsKey(key);
			}
			return result;
		}

		public bool Set(string key, string value)
		{
			bool result;
			using (RedisClient redisClient = this.GetRedisClient())
			{
				result = redisClient.Set<string>(key, value);
			}
			return result;
		}

		public void AddItem(string key, string value)
		{
			using (RedisClient redisClient = this.GetRedisClient())
			{
				redisClient.Set<string>(key, value);
			}
		}

		public string GetValue(string key)
		{
			string result;
			using (RedisClient redisClient = this.GetRedisClient())
			{
				result = (redisClient.Get<string>(key) ?? string.Empty);
			}
			return result;
		}

		public bool Set<T>(string key, T value)
		{
			bool result;
			using (RedisClient redisClient = this.GetRedisClient())
			{
				result = redisClient.Set<T>(key, value);
			}
			return result;
		}

		public bool Set<T>(string key, T value, DateTime expiresAt)
		{
			bool result;
			using (RedisClient redisClient = this.GetRedisClient())
			{
				result = redisClient.Set<T>(key, value, expiresAt);
			}
			return result;
		}

		public bool Add<T>(string key, T value)
		{
			bool result;
			using (RedisClient redisClient = this.GetRedisClient())
			{
				result = redisClient.Add<T>(key, value);
			}
			return result;
		}

		public T Get<T>(string key)
		{
			T result;
			using (RedisClient redisClient = this.GetRedisClient())
			{
				result = redisClient.Get<T>(key);
			}
			return result;
		}

		public long GetListCount(string key)
		{
			long listCount;
			using (RedisClient redisReadClient = this.GetRedisReadClient())
			{
				listCount = redisReadClient.GetListCount(key);
			}
			return listCount;
		}

		public List<string> GetAllFromList(string key)
		{
			List<string> allItemsFromList;
			using (RedisClient redisReadClient = this.GetRedisReadClient())
			{
				allItemsFromList = redisReadClient.GetAllItemsFromList(key);
			}
			return allItemsFromList;
		}

		public void SetItemInList(string key, int idx, string value)
		{
			using (RedisClient redisClient = this.GetRedisClient())
			{
				redisClient.SetItemInList(key, idx, value);
			}
		}

		public string GetValueFormHash(string hashId, string key)
		{
			string valueFromHash;
			using (RedisClient redisReadClient = this.GetRedisReadClient())
			{
				valueFromHash = redisReadClient.GetValueFromHash(hashId, key);
			}
			return valueFromHash;
		}

		public bool SetEntryInHash(string hashId, string key, string value)
		{
			bool result;
			using (RedisClient redisClient = this.GetRedisClient())
			{
				result = redisClient.SetEntryInHash(hashId, key, value);
			}
			return result;
		}

		public bool HashContainsEntry(string hashId, string key)
		{
			bool result;
			using (RedisClient redisReadClient = this.GetRedisReadClient())
			{
				result = redisReadClient.HashContainsEntry(hashId, key);
			}
			return result;
		}

		public bool RemoveEntryFromHash(string hashId, string key)
		{
			bool result;
			using (RedisClient redisClient = this.GetRedisClient())
			{
				result = redisClient.RemoveEntryFromHash(hashId, key);
			}
			return result;
		}

		public T ReturnMethod<T>(Func<RedisClient, T> func, RedisWriteReadEnum type)
		{
			RedisClient redisClient = null;
			T result;
			try
			{
				if (type == RedisWriteReadEnum.Read)
				{
					redisClient = this.GetRedisReadClient();
				}
				if (type == RedisWriteReadEnum.Write)
				{
					redisClient = this.GetRedisClient();
				}
				result = func(redisClient);
				return result;
			}
			catch (Exception var_1_6E)
			{
			}
			finally
			{
				if (redisClient != null)
				{
					redisClient.Dispose();
				}
			}
			result = default(T);
			return result;
		}

		public bool Remove(string cacheKey)
		{
			return this.ReturnMethod<bool>((RedisClient client) => client.Remove(cacheKey), RedisWriteReadEnum.Write);
		}

		public void RemoveByKeys(IEnumerable<string> keys)
		{
			using (RedisClient redisClient = this.GetRedisClient())
			{
				redisClient.RemoveAll(keys);
			}
		}

		public void RemoveItemFromList(string key, string value)
		{
			using (RedisClient redisClient = this.GetRedisClient())
			{
				redisClient.RemoveItemFromList(key, value);
			}
		}

		public void RemoveByRegex(string pattern)
		{
			using (RedisClient redisClient = this.GetRedisClient())
			{
				redisClient.RemoveAll(redisClient.SearchKeys(pattern));
			}
		}

		public void ClearAll()
		{
			using (RedisClient redisClient = this.GetRedisClient())
			{
				redisClient.FlushAll();
			}
		}
	}
}
