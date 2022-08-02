using Ao.Cache;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Structing.Idempotent.Services
{
    public class IdempotentService : IIdempotentService
    {
        public ILockerFactory LockerFactory { get; }

        public ICacheVisitor CacheVisitor { get; }

        public IIdempotentKeyGenerator IdempotentKeyGenerator { get; }

        public IdempotentService(ILockerFactory lockerFactory,
            ICacheVisitor cacheVisitor,
            IIdempotentKeyGenerator idempotentkeyGenerator)
        {
            LockerFactory = lockerFactory;
            CacheVisitor = cacheVisitor;
            IdempotentKeyGenerator = idempotentkeyGenerator;
        }
        public Task<bool> DeleteIdempotentAsync(Type targetType, string methodName, params object[] args)
        {
            return DeleteIdempotentAsync(targetType, targetType.GetMethod(methodName), args);
        }
        public Task<bool> DeleteIdempotentAsync(Type targetType, MethodInfo method, params object[] args)
        {
            var header = IdempotentKeyGenerator.GetHeader(targetType, method);
            var key = IdempotentKeyGenerator.GetKey(header, args);
            return CacheVisitor.DeleteAsync(key);
        }

        public Task<IdempotentToken<T>> IdempotentAsync<T>(string resourceKey)
        {
            return IdempotentAsync<T>(resourceKey, Consts.DefaultLockExpireTime, Consts.DefaultRequestTime, CacheSetIf.NotExists);
        }
        public Task<IdempotentToken<T>> IdempotentAsync<T>(string resourceKey, TimeSpan keyExpireTime)
        {
            return IdempotentAsync<T>(resourceKey, keyExpireTime, Consts.DefaultRequestTime, CacheSetIf.NotExists);
        }

        public Task<IdempotentToken<T>> IdempotentAsync<T>(string resourceKey, TimeSpan keyExpireTime, TimeSpan? resultCacheTime)
        {
            return IdempotentAsync<T>(resourceKey, keyExpireTime, resultCacheTime, CacheSetIf.NotExists);
        }
        public async Task<IdempotentToken<T>> IdempotentAsync<T>(string resourceKey, TimeSpan keyExpireTime, TimeSpan? resultCacheTime, CacheSetIf cacheSetIf)
        {
            var resCache = await CacheVisitor.GetAsync<T>(resourceKey);
            if (resCache != null)
            {
                return IdempotentToken<T>.FromResult(resCache);
            }
            var lockKey = string.Concat("Lock.", resourceKey);
            var locker = await LockerFactory.CreateLockAsync(lockKey, keyExpireTime);
            resCache = await CacheVisitor.GetAsync<T>(resourceKey);
            if (resCache != null)
            {
                return IdempotentToken<T>.FromResult(resCache);
            }

            return new IdempotentToken<T>(
                resourceKey, locker, CacheVisitor, resultCacheTime, cacheSetIf, false, default);
        }
    }
}
