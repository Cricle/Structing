using SecurityLogin;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Structing.Idempotent.Services
{
    public interface IIdempotentService
    {
        Task<bool> DeleteIdempotentAsync(Type targetType, string methodName, params object[] args);
        Task<bool> DeleteIdempotentAsync(Type targetType, MethodInfo method, params object[] args);

        Task<IdempotentToken<T>> IdempotentAsync<T>(string resourceKey);
        Task<IdempotentToken<T>> IdempotentAsync<T>(string resourceKey, TimeSpan keyExpireTime);
        Task<IdempotentToken<T>> IdempotentAsync<T>(string resourceKey, TimeSpan keyExpireTime, TimeSpan? resultCacheTime);
        Task<IdempotentToken<T>> IdempotentAsync<T>(string resourceKey, TimeSpan keyExpireTime, TimeSpan? resultCacheTime, CacheSetIf cacheSetIf);
    }
}