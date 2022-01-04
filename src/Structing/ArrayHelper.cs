using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Structing
{
    internal static class ArrayHelper<T>
    {
#if NET452
        private static readonly T[] EmptyArray = new T[0];
#endif
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[] Empty()
        {
#if NET452
            return EmptyArray;
#else
            return Array.Empty<T>();
#endif
        }
    }
}
