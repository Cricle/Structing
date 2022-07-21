using SecurityLogin;
using System;
using System.Threading.Tasks;

namespace Structing.Idempotent.Services
{
    public interface IIdempotentService
    {
        Task<IdempotentToken<T>> IdempotentAsync<T>(string resourceKey);
        Task<IdempotentToken<T>> IdempotentAsync<T>(string resourceKey, TimeSpan keyExpireTime);
        Task<IdempotentToken<T>> IdempotentAsync<T>(string resourceKey, TimeSpan keyExpireTime, TimeSpan? resultCacheTime);
        Task<IdempotentToken<T>> IdempotentAsync<T>(string resourceKey, TimeSpan keyExpireTime, TimeSpan? resultCacheTime, CacheSetIf cacheSetIf);
    }
}