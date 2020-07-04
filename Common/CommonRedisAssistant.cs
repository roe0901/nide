using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class CommonRedisAssistant
    {
		private static object objLock = new object();

		private static CommonRedisAssistant _instance = null;

		private RedisHelper redis;

		public static CommonRedisAssistant Instance
		{
			get
			{
				if (CommonRedisAssistant._instance == null)
				{
					lock (CommonRedisAssistant.objLock)
					{
						if (CommonRedisAssistant._instance == null)
						{
							CommonRedisAssistant._instance = new CommonRedisAssistant();
						}
					}
				}
				return CommonRedisAssistant._instance;
			}
		}

		private CommonRedisAssistant()
		{
			this.redis = RedisHelper.GetCommonInstance;
		}

		public bool Set(string key, string value)
		{
			return this.redis.Set(key, value);
		}

		public void Add(string key, string value)
		{
			using (RedisClient redisClient = this.redis.GetRedisClient())
			{
				redisClient.Set<string>(key, value);
			}
		}

		public bool Set<T>(string key, T value)
		{
			return this.redis.Set<T>(key, value);
		}

		public bool Set<T>(string key, T value, DateTime expiresAt)
		{
			return this.redis.Set<T>(key, value, expiresAt);
		}

		public bool Add<T>(string key, T value)
		{
			return this.redis.Add<T>(key, value);
		}

		public T Get<T>(string key)
		{
			return this.redis.Get<T>(key);
		}

		public bool SetExpire(string key, TimeSpan ts)
		{
			return this.redis.ExpireEntryIn(key, ts);
		}

		public void AddOnList(string key, string value)
		{
			using (RedisClient redisClient = this.redis.GetRedisClient())
			{
				redisClient.RemoveItemFromList(key, value);
				redisClient.EnqueueItemOnList(key, value);
			}
		}

		public void AddOnList(string key, List<string> list)
		{
			using (RedisClient redisClient = this.redis.GetRedisClient())
			{
				redisClient.AddRangeToList(key, list);
			}
		}

		public void InsertInList(string key, int index, string value)
		{
			this.redis.SetItemInList(key, index, value);
		}

		public void InsertLastInList(string key, string value)
		{
			long listCount = this.redis.GetListCount(key);
			int idx = (listCount > 0L) ? ((int)listCount - 1) : 1;
			this.redis.SetItemInList(key, idx, value);
		}

		public string GetItemInList(string key)
		{
			return this.redis.DequeueItem(key);
		}

		public string GetValueFormHash(string hashId, string key)
		{
			return this.redis.GetValueFormHash(hashId, key);
		}

		public bool SetEntryInHash(string hashId, string key, string value)
		{
			return this.redis.SetEntryInHash(hashId, key, value);
		}

		public bool HashContainsEntry(string hashId, string key)
		{
			return this.redis.HashContainsEntry(hashId, key);
		}

		public bool RemoveEntryFromHash(string hashId, string key)
		{
			return this.redis.RemoveEntryFromHash(hashId, key);
		}

		public void Remove(string key)
		{
			using (RedisClient redisClient = this.redis.GetRedisClient())
			{
				redisClient.Remove(key);
			}
		}

		public void RemoveAll(IEnumerable<string> keys)
		{
			this.redis.RemoveByKeys(keys);
		}

		public void ClearAll()
		{
			this.redis.ClearAll();
		}

		public long GetListCount(string key)
		{
			return this.redis.GetListCount(key);
		}

		public List<string> GetRangeFromList(string key)
		{
			return this.redis.GetRangeFromList(key);
		}

		public List<string> GetAllFromList(string key)
		{
			return this.redis.GetAllFromList(key);
		}

		public void AddCount(string key)
		{
			lock (key)
			{
				string text = this.redis.GetValue(key);
				this.redis.Remove(key);
				if (string.IsNullOrEmpty(text))
				{
					text = "0";
				}
				int num = int.Parse(text) + 1;
				this.redis.AddItem(key, num.ToString());
			}
		}

		public bool IsContainsKey(string key)
		{
			return this.redis.IsContainsKey(key);
		}

		public string GetValue(string key)
		{
			return this.redis.GetValue(key);
		}

		public void RemoveItemFromList(string key, string value)
		{
			this.redis.RemoveItemFromList(key, value);
		}

		public void RemoveByRegex(string pattern)
		{
			this.redis.RemoveByRegex(pattern);
		}
	}
}
