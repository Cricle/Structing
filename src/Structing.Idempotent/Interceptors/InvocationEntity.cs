using System;
using System.Diagnostics;
using System.Reflection;

namespace Structing.Idempotent.Interceptors
{
    readonly struct InvocationEntity : IEquatable<InvocationEntity>
    {
        internal InvocationEntity(Type targetType, MethodInfo methodInfo)
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
}
