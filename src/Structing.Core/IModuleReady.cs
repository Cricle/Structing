using System.Threading.Tasks;

namespace Structing.Core
{
    /// <summary>
    /// 标识模块准备
    /// </summary>
    public interface IModuleReady
    {
        /// <summary>
        /// 准备之前
        /// </summary>
        /// <param name="context">上下文</param>
        /// <returns></returns>
        Task BeforeReadyAsync(IReadyContext context);
        /// <summary>
        /// 调用以准备模块
        /// </summary>
        /// <param name="context">上下文</param>
        /// <returns></returns>
        Task ReadyAsync(IReadyContext context);
        /// <summary>
        /// 准备之后
        /// </summary>
        /// <param name="context">上下文</param>
        /// <returns></returns>
        Task AfterReadyAsync(IReadyContext context);
    }
}
