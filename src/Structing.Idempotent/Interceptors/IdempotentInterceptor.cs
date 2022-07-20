using Castle.DynamicProxy;
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
        readonly struct InvocationEntity : IEquatable<InvocationEntity>
        {
            public InvocationEntity(Type targetType, MethodInfo methodInfo)
            {
                Debug.Assert(targetType != null);
                Debug.Assert(methodInfo != null);
                TargetType = targetType;
                MethodInfo = methodInfo;
            }

            public Type TargetType { get; }

            public MethodInfo MethodInfo { get; }

            public override bool Equals(object obj)
            {
                return Equals((InvocationEntity)obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    var h = 31 * 17 + TargetType.GetHashCode();
                    return h * 31 + MethodInfo.GetHashCode();
                }
            }

            public bool Equals(InvocationEntity other)
            {
                return other.TargetType == TargetType &&
                    other.MethodInfo == MethodInfo;
            }
            public override string ToString()
            {
                return $"{{TargetType: {TargetType}, MethodInfo: {MethodInfo}}}";
            }
        }
        class InvocationValues
        {
            public string HeaderKey { get; set; }

            public IdempotentAttribute IdempotentAttribute { get; set; }

            public TimeSpan? ResultCacheTime { get; set; }

            public int[] UsedArgIndexs { get; set; }

            public bool Skip { get; set; }
        }

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
            return proceed(invocation, proceedInfo);//没返回值的不处理
        }

        protected override async Task<TResult> InterceptAsync<TResult>(IInvocation invocation, IInvocationProceedInfo proceedInfo, Func<IInvocation, IInvocationProceedInfo, Task<TResult>> proceed)
        {
            var entity = new InvocationEntity(invocation.TargetType, invocation.Method);
            if (!invocationValueMap.TryGetValue(entity, out var values))
            {
                var indemAttr = GetAntiReentry(invocation);
                if (indemAttr == null)
                {
                    values = new InvocationValues { Skip = true };
                    invocationValueMap[entity] = values;
                }
                else
                {
                    var header = TypeNameHelper.GetFriendlyFullName(invocation.TargetType) + "." + invocation.Method.Name;
                    var resultCacheTime = indemAttr.ResultCacheForever ? null : (TimeSpan?)indemAttr.ResultCacheTime;
                    var pars = invocation.Method.GetParameters();
                    var useIndexs = new List<int>(pars.Length);
                    for (int i = 0; i < pars.Length; i++)
                    {
                        if (pars[i].GetCustomAttribute<SkipKeyPartAttribute>() == null)
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
            using (var tk = await IdempotentService.AntiReentryAsync<TResult>(key, Consts.DefaultLockExpireTime, values.ResultCacheTime))
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
        private static IdempotentAttribute GetAntiReentry(IInvocation invocation)
        {
            return invocation.TargetType.GetCustomAttribute<IdempotentAttribute>() ??
                invocation.Method.GetCustomAttribute<IdempotentAttribute>();
        }
    }
}
