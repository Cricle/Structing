using System;
using System.Runtime.CompilerServices;

namespace Structing.Core
{
    internal static class ArrayHelper<T>
    {
#if NETSTANDARD1_0
        private static readonly T[] EmptyArray = new T[0];
#endif
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[] Empty()
        {
#if NETSTANDARD1_0
            return EmptyArray;
#else
            return Array.Empty<T>();
#endif
        }
    }
}
