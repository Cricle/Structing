namespace Structing
{
    public interface IModuleRegister
    {
        void ReadyRegister(IRegisteContext context);
        void Register(IRegisteContext context);
        void AfterRegister(IRegisteContext context);
    }
}
