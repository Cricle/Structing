using Castle.DynamicProxy;

namespace Structing.DryInterceptor
{
    public class AsyncInterceptor<T> : AsyncDeterminationInterceptor where T : IAsyncInterceptor
    {
        public AsyncInterceptor(T asyncInterceptor) : base(asyncInterceptor) { }
    }
}
