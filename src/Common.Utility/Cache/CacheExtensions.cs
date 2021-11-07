using Common.Utility.Cache.Core;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Common.Utility.Cache
{
    public static class CacheExtensions
    {
        /// <summary>
        /// 添加缓存功能
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddMemoryCache(this IServiceCollection services)
        {
            services.AddMemoryCache(options =>
            {
                // SizeLimit缓存是没有大小的，此值设置缓存的份数
                // 注意：netcore中的缓存是没有单位的，缓存项和缓存的相对关系
                options.SizeLimit = 1024;
                // 缓存满的时候压缩20%的优先级较低的数据
                options.CompactionPercentage = 0.2;
                // 两秒钟查找一次过期项
                options.ExpirationScanFrequency = TimeSpan.FromSeconds(2);
            });

            // MemoryCache缓存注入
            services.AddTransient<ICacheService, MemoryCacheService>();

            return services;
        }

        /// <summary>
        /// 添加缓存功能
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddMemoryCache(this IServiceCollection services, Action<MemoryCacheOptions> memoryCacheOptions)
        {
            if (memoryCacheOptions == null)
            {
                throw new ArgumentNullException(nameof(MemoryCacheOptions));
            }
            services.AddMemoryCache(memoryCacheOptions);

            // MemoryCache缓存注入
            services.AddTransient<ICacheService, MemoryCacheService>();

            return services;
        }
    }
}
