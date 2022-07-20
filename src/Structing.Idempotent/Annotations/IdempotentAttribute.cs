using Structing.Idempotent.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Structing.Idempotent.Annotations
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public sealed class IdempotentAttribute : Attribute
    {
        public TimeSpan ResultCacheTime { get; set; } = Consts.DefaultRequestTime;

        public bool ResultCacheForever { get; set; }
    }
}
