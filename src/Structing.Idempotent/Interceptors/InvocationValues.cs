using Structing.Idempotent.Annotations;
using System;

namespace Structing.Idempotent.Interceptors
{
    class InvocationValues
    {
        public string HeaderKey { get; set; }

        public IdempotentAttribute IdempotentAttribute { get; set; }

        public TimeSpan? ResultCacheTime { get; set; }

        public int[] UsedArgIndexs { get; set; }

        public bool Skip { get; set; }

        public bool FullArgs { get; set; }
    }
}
