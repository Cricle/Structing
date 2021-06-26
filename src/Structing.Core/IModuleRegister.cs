﻿namespace Structing.Core
{
    public interface IModuleRegister
    {
        void ReadyRegister(IRegisteContext context);
        void Register(IRegisteContext context);
    }
}
