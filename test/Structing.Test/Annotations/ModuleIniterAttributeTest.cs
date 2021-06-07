using Microsoft.VisualStudio.TestTools.UnitTesting;
using Structing.Annotations;
using Structing.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Structing.Test.Annotations
{
    [TestClass]
    public class ModuleIniterAttributeTest
    {
        class AModuleIniter : IModuleInit
        {

            public static bool IsInvokeAsync { get; set; }
            public Task InvokeAsync(IReadyContext context)
            {
                IsInvokeAsync = true;
                return Task.FromResult(1);
            }
        }
        [TestMethod]
        public async Task GivenNoModuleInitType_MustThrowException()
        {
            await Assert.ThrowsExceptionAsync<InvalidOperationException>(() => new ModuleIniterAttribute().ReadyAsync(null, typeof(object)));
        }
        [TestMethod]
        public async Task ModuleIniter_Ready_MustInvoked()
        {
            var attr = new ModuleIniterAttribute();
            await attr.ReadyAsync(null, typeof(AModuleIniter));

            Assert.IsTrue(AModuleIniter.IsInvokeAsync);
        }
    }
}
