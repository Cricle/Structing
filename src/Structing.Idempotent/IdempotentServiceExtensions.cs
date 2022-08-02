﻿using Structing.Idempotent;
using Structing.Idempotent.Interceptors;
using Structing.Idempotent.Services;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class IdempotentServiceExtensions
    {
        public static IServiceCollection AddIdempotent(this IServiceCollection services)
        {
            services.AddScoped<IIdempotentService, IdempotentService>();
            services.AddSingleton<IIdempotentKeyGenerator>(DefaultIdempotentKeyGenerator.Instance);
            services.AddScoped<IdempotentInterceptor>();
            return services;
        }
    }
}
