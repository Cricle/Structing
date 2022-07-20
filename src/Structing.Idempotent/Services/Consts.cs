using SecurityLogin;
using System;
using System.Threading.Tasks;

namespace Structing.Idempotent.Services
{
    public static class Consts
    {
        public static readonly TimeSpan DefaultLockExpireTime = TimeSpan.FromSeconds(10);

        public static readonly TimeSpan DefaultRequestTime = TimeSpan.FromMinutes(3);

        public static Task<ILocker> CreateLockAsync(this ILockerFactory factory, string resource)
        {
            return factory.CreateLockAsync(resource, DefaultLockExpireTime);
        }

    }

}
