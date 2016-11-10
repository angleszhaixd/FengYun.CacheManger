using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace FengYun.CacheManager.Core
{
    /// <summary>
    /// Web.confi中配置的缓存名称 
    /// 配置文件中的缓存名称严格按照如下名称定义
    /// </summary>
    public class CacheConfigNames
    {
        /// <summary>
        /// 多级缓存配置名称
        /// 使用SystemRuntimeCaching+Redis 进行缓存
        /// </summary>
        public const string multiLayerCache = "multiCache";

        /// <summary>
        /// 系统内存缓存
        /// 使用SystemRuntimeCaching
        /// </summary>
        public const string runtimeMemoryCache = "systemRuntimeCache";

        /// <summary>
        /// 系统字典缓存
        /// 使用ConcurrentDictionary
        /// </summary>
        public const string dictionaryCache = "dictionaryCache";

        /// <summary>
        /// redis分布式缓存
        /// 使用StackExchange.Redis
        /// </summary>
        public const string redisCache = "redisCache";

        public static List<string> AllCacheConfigNames = new List<string>() { multiLayerCache, runtimeMemoryCache, dictionaryCache, redisCache };
    }
}
