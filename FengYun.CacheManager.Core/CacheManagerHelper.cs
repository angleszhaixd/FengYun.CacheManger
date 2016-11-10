using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CacheManager.Core;
using System.Collections.Concurrent;

namespace FengYun.CacheManager.Core
{
    /// <summary>
    /// CacheManager帮助类 
    /// 获取Cache实例
    /// Author:翟晓东
    /// Date:2016.11.8
    /// </summary>
    public class CacheManagerHelper
    {
        //private static readonly string _configCacheName = ConfigurationManager.AppSettings["CacheManagerConfigName"].ToString()?? "multiCacheName";
        private static readonly object Locker = new object();
        /// <summary>
        /// 缓存CacheManager配置信息
        /// </summary>
        private static readonly ConcurrentDictionary<string, Lazy<CacheManagerConfiguration>> _cacheManagerConfigs = new ConcurrentDictionary<string, Lazy<CacheManagerConfiguration>>();
        /// <summary>
        /// 缓存创建的CacheManager实例
        /// </summary>
        private static readonly ConcurrentDictionary<string, Object> _cacheManagers = new ConcurrentDictionary<string, Object>();
        static CacheManagerHelper()
        {
            foreach (string configName in CacheConfigNames.AllCacheConfigNames)
            {
                _cacheManagerConfigs.TryAdd(configName, new Lazy<CacheManagerConfiguration>(
                    () =>
                    {
                        var cfg = ConfigurationBuilder.LoadConfiguration(configName);
                        cfg.Builder.WithNLogLogging();
                        return cfg;
                    }
                ));
            }
        }
        #region 多级缓存实例
        /// <summary>
        /// 默认缓存实例(多级缓存)
        /// </summary>
        public static ICacheManager<object> Default
        {
            get { return CacheManagerInstance<object>(CacheConfigNames.multiLayerCache); }
        }
        /// <summary>
        /// 多级缓存实例
        /// </summary>
        /// <typeparam name="TCacheValue">缓存值类型</typeparam>
        /// <returns></returns>
        public static ICacheManager<TCacheValue> MultliCacheFor<TCacheValue>()
        {
            return CacheManagerInstance<TCacheValue>(CacheConfigNames.multiLayerCache);
        }
        #endregion

        #region 系统内存缓存
        /// <summary>
        /// 系统内存缓存实例(value值类型Object)
        /// </summary>
        /// <returns></returns>
        public static ICacheManager<object> MemoryCache
        {
            get { return CacheManagerInstance<object>(CacheConfigNames.runtimeMemoryCache); }
        }
        /// <summary>
        /// 系统内存缓存实例
        /// </summary>
        /// <typeparam name="TCacheValue">缓存值类型</typeparam>
        /// <returns></returns>
        public static ICacheManager<TCacheValue> MemoryCacheFor<TCacheValue>()
        {
            return CacheManagerInstance<TCacheValue>(CacheConfigNames.runtimeMemoryCache);
        }
        /// <summary>
        /// ConcurrentDictionary字典缓存实例(value值类型Object)
        /// </summary>
        /// <returns></returns>
        public static ICacheManager<object> DictionaryCache
        {
            get { return CacheManagerInstance<object>(CacheConfigNames.dictionaryCache); }
        }
        /// <summary>
        /// ConcurrentDictionary字典缓存实例
        /// </summary>
        /// <typeparam name="TCacheValue">缓存值类型</typeparam>
        /// <returns></returns>
        public static ICacheManager<TCacheValue> DictionaryCacheFor<TCacheValue>()
        {
            return CacheManagerInstance<TCacheValue>(CacheConfigNames.dictionaryCache);
        }
        #endregion

        #region Redis分布式缓存
        /// <summary>
        /// Redis分布式缓存实例(value值类型Object)
        /// </summary>
        /// <returns></returns>
        public static ICacheManager<object> RedisCache
        {
            get { return CacheManagerInstance<object>(CacheConfigNames.redisCache); }
        }
        /// <summary>
        /// Redis分布式缓存实例
        /// </summary>
        /// <typeparam name="TCacheValue">缓存值类型</typeparam>
        /// <returns></returns>
        public static ICacheManager<TCacheValue> RedisCacheFor<TCacheValue>()
        {
            return CacheManagerInstance<TCacheValue>(CacheConfigNames.redisCache);
        }
        #endregion

        /// <summary>
        /// 获取CacheManager实例
        /// </summary>
        /// <typeparam name="TCacheValue">缓存值的类型</typeparam>
        /// <param name="cacheName">缓存配置名称<see cref="CacheConfigNames"/>（默认使用内存缓存<see cref="CacheConfigNames.runtimeMemoryCache"/>）</param>
        /// <returns></returns>
        private static ICacheManager<TCacheValue> CacheManagerInstance<TCacheValue>(string cacheName)
        {
            cacheName = cacheName ?? CacheConfigNames.runtimeMemoryCache;
            string instanceName = GetCacheInstanceName(cacheName, typeof(TCacheValue));
            if (!_cacheManagers.ContainsKey(instanceName))
            {
                lock (Locker)
                {
                    if (!_cacheManagers.ContainsKey(instanceName))
                    {
                        var cfg = GetCacheManagerConfig(cacheName);
                        _cacheManagers[instanceName] = CacheFactory.FromConfiguration<TCacheValue>(instanceName, cfg);
                    }
                }
            }
            return _cacheManagers[instanceName] as ICacheManager<TCacheValue>;
        }

        /// <summary>
        /// 获取缓存配置信息<see cref="CacheConfigNames"/>
        /// </summary>
        /// <param name="cacheName">缓存配置名称<see cref="CacheConfigNames"/></param>
        /// <returns></returns>
        public static CacheManagerConfiguration GetCacheManagerConfig(string cacheName)
        {
            return _cacheManagerConfigs[cacheName]?.Value;
            //if (_cacheManagerConfigs.ContainsKey(cacheName))
            //    return _cacheManagerConfigs[cacheName].Value;
            //return null;
        }

        /// <summary>
        /// 获取缓存实例名称
        /// </summary>
        /// <param name="cacheName"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        private static string GetCacheInstanceName(string cacheName, Type t)
        {
            return string.Format("{0}.{1}", cacheName, t.Name);
        }
    }
}
