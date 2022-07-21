using Castle.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;
using SecurityLogin;
using Structing.Idempotent.Annotations;
using Structing.Idempotent.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Structing.Idempotent.Interceptors
{
    public class IdempotentInterceptor : AsyncInterceptorBase
    {
        private static readonly Dictionary<InvocationEntity, InvocationValues> invocationValueMap = new Dictionary<InvocationEntity, InvocationValues>();

        public IIdempotentKeyGenerator IdempotentKeyGenerator { get; }

        public IIdempotentService IdempotentService { get; }

        public IdempotentInterceptor(IIdempotentService idempotentService,
            IIdempotentKeyGenerator idempotentKeyGenerator)
        {
            IdempotentService = idempotentService ?? throw new ArgumentNullException(nameof(idempotentService));
            IdempotentKeyGenerator = idempotentKeyGenerator ?? throw new ArgumentNullException(nameof(idempotentKeyGenerator));
        }

        protected override Task InterceptAsync(IInvocation invocation, IInvocationProceedInfo proceedInfo, Func<IInvocation, IInvocationProceedInfo, Task> proceed)
        {
            return proceed(invocation, proceedInfo);
        }

        protected override async Task<TResult> InterceptAsync<TResult>(IInvocation invocation, IInvocationProceedInfo proceedInfo, Func<IInvocation, IInvocationProceedInfo, Task<TResult>> proceed)
        {
            var entity = new InvocationEntity(invocation.TargetType, invocation.Method);
            if (!invocationValueMap.TryGetValue(entity, out var values))
            {
                var indemAttr = GetIdempotent(invocation);
                if (indemAttr == null)
                {
                    values = new InvocationValues { Skip = true };
                    invocationValueMap[entity] = values;
                }
                else
                {
                    var header = IdempotentKeyGenerator.GetHeader(invocation.TargetType, invocation.Method);
                    var resultCacheTime = indemAttr.ResultCacheForever ? null : (TimeSpan?)indemAttr.ResultCacheTime;
                    var pars = invocation.Method.GetParameters();
                    var useIndexs = new List<int>(pars.Length);
                    for (int i = 0; i < pars.Length; i++)
                    {
                        if (pars[i].GetCustomAttribute<IdempotentSkipKeyPartAttribute>() == null)
                        {
                            useIndexs.Add(i);
                        }
                    }
                    values = new InvocationValues
                    {
                        HeaderKey = header,
                        IdempotentAttribute = indemAttr,
                        ResultCacheTime = resultCacheTime,
                        UsedArgIndexs = useIndexs.ToArray()
                    };
                    invocationValueMap[entity] = values;
                }
            }
            if (values.Skip)
            {
                return await proceed(invocation, proceedInfo);
            }
            var args = new object[values.UsedArgIndexs.Length];
            for (int i = 0; i < values.UsedArgIndexs.Length; i++)
            {
                args[i] = invocation.Arguments[i];
            }
            var key = IdempotentKeyGenerator.GetKey(values.HeaderKey, args);
            using (var tk = await IdempotentService.IdempotentAsync<TResult>(key, Consts.DefaultLockExpireTime, values.ResultCacheTime))
            {
                if (tk.HasResult)
                {
                    return tk.Result;
                }

                var result = await proceed(invocation, proceedInfo);
                await tk.SetAsync(result);
                return result;
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static IdempotentAttribute GetIdempotent(IInvocation invocation)
        {
            return invocation.TargetType.GetCustomAttribute<IdempotentAttribute>() ??
                invocation.Method.GetCustomAttribute<IdempotentAttribute>();
        }
    }
}
