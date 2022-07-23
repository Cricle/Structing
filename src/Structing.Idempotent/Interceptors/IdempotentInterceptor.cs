using Castle.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;
using SecurityLogin;
using Structing.Idempotent.Annotations;
using Structing.Idempotent.Models;
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
                        UsedArgIndexs = useIndexs.ToArray(),
                        FullArgs = useIndexs.Count == pars.Length
                    };
                    invocationValueMap[entity] = values;
                }
            }
            if (values.Skip)
            {
                var res = await proceed(invocation, proceedInfo);
                if (res is IdempotentBase idem)
                {
                    idem.Skip = true;
                    idem.Args = invocation.Arguments;
                }
                return res;
            }
            var args = invocation.Arguments;
            if (!values.FullArgs)
            {
                args = new object[values.UsedArgIndexs.Length];
                for (int i = 0; i < values.UsedArgIndexs.Length; i++)
                {
                    args[i] = invocation.Arguments[values.UsedArgIndexs[i]];
                }
            }
            var key = IdempotentKeyGenerator.GetKey(values.HeaderKey, args);
            using (var tk = await IdempotentService.IdempotentAsync<TResult>(key, Consts.DefaultLockExpireTime, values.ResultCacheTime))
            {
                if (tk.HasResult)
                {
                    if (tk.Result is IdempotentBase idem)
                    {
                        idem.Args = args;
                        idem.Status = IdempotentStatus.IdempotentHit;
                        idem.CacheKey = key;
                    }
                    return tk.Result;
                }

                var result = await proceed(invocation, proceedInfo);
                if (result is IdempotentBase ridem)
                {
                    ridem.Args = args;
                    ridem.Status = IdempotentStatus.MethodHit;
                    ridem.CacheKey = key;
                }
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
