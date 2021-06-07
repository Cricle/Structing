namespace Structing.Core
{
    /// <summary>
    /// 模块注册器
    /// </summary>
    public interface IModuleRegister
    {
        /// <summary>
        /// 准备注册系统
        /// </summary>
        /// <param name="context"></param>
        void ReadyRegister(IRegisteContext context);
        /// <summary>
        /// 调用以注册服务
        /// </summary>
        /// <param name="registerContext">注册上下文</param>
        void Register(IRegisteContext context);
    }
}
