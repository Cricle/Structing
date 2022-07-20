using SecurityLogin;
using System;
using System.Threading.Tasks;

namespace Structing.Idempotent.Services
{
    public class IdempotentService : IIdempotentService
    {
        private readonly ILockerFactory lockerFactory;
        private readonly ICacheVisitor cacheVisitor;

        public IdempotentService(ILockerFactory lockerFactory, ICacheVisitor cacheVisitor)
        {
            this.lockerFactory = lockerFactory;
            this.cacheVisitor = cacheVisitor;
        }
        public Task<IdempotentToken<T>> AntiReentryAsync<T>(string resourceKey)
        {
            return AntiReentryAsync<T>(resourceKey, Consts.DefaultLockExpireTime, Consts.DefaultRequestTime, CacheSetIf.NotExists);
        }
        public Task<IdempotentToken<T>> AntiReentryAsync<T>(string resourceKey, TimeSpan keyExpireTime)
        {
            return AntiReentryAsync<T>(resourceKey, keyExpireTime, Consts.DefaultRequestTime, CacheSetIf.NotExists);
        }

        public Task<IdempotentToken<T>> AntiReentryAsync<T>(string resourceKey, TimeSpan keyExpireTime, TimeSpan? resultCacheTime)
        {
            return AntiReentryAsync<T>(resourceKey, keyExpireTime, resultCacheTime, CacheSetIf.NotExists);
        }
        public async Task<IdempotentToken<T>> AntiReentryAsync<T>(string resourceKey, TimeSpan keyExpireTime, TimeSpan? resultCacheTime, CacheSetIf cacheSetIf)
        {
            var resCache = await cacheVisitor.GetAsync<T>(resourceKey);
            if (resCache != null)
            {
                return IdempotentToken<T>.FromResult(resCache);
            }
            var lockKey = string.Concat("Lock.", resourceKey);
            var locker = await lockerFactory.CreateLockAsync(lockKey, keyExpireTime);
            resCache = await cacheVisitor.GetAsync<T>(resourceKey);
            if (resCache != null)
            {
                return IdempotentToken<T>.FromResult(resCache);
            }

            return new IdempotentToken<T>(
                resourceKey, locker, cacheVisitor, resultCacheTime, cacheSetIf, false, default);
        }
    }
}
