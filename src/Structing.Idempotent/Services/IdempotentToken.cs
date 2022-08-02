using Ao.Cache;
using System;
using System.Threading.Tasks;

namespace Structing.Idempotent.Services
{
    public readonly struct IdempotentToken<T> : IDisposable
    {
        public static IdempotentToken<T> FromResult(T result)
        {
            return new IdempotentToken<T>(null, null, null, null, default, true, result);
        }

        public IdempotentToken(string key, ILocker locker, ICacheVisitor cacheVisitor, TimeSpan? resultCacheTime, CacheSetIf cacheSetIf, bool hasResult, T result)
        {
            Key = key;
            Locker = locker;
            CacheVisitor = cacheVisitor;
            ResultCacheTime = resultCacheTime;
            CacheSetIf = cacheSetIf;
            HasResult = hasResult;
            Result = result;
        }

        public string Key { get; }

        public ILocker Locker { get; }

        public ICacheVisitor CacheVisitor { get; }

        public TimeSpan? ResultCacheTime { get; }

        public CacheSetIf CacheSetIf { get; }

        public bool HasResult { get; }

        public T Result { get; }

        public Task SetAsync(T value)
        {
            return CacheVisitor.SetAsync(Key, value, ResultCacheTime, CacheSetIf);
        }

        public void Dispose()
        {
            Locker?.Dispose();
        }
    }

}
