using Microsoft.VisualStudio.TestTools.UnitTesting;
using Structing.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Structing.Core.Test.Annotations
{
    [TestClass]
    public class ModulePartAttributeTest
    {
        class MockModelPart : IModulePart
        {
            public static bool IsInvoked { get; set; }

            public void Invoke(IRegisteContext context)
            {
                IsInvoked = true;
            }
        }
        [TestMethod]
        public void NotImplIModulePart_MustThrowInvalidOperationException()
        {
            Assert.ThrowsException<InvalidOperationException>(()=>new ModulePartAttribute().Register(null,typeof(object)));
        }
        [TestMethod]
        public void Regist_MustRun()
        {
            MockModelPart.IsInvoked = false;
            new ModulePartAttribute().Register(null, typeof(MockModelPart));
            Assert.IsTrue(MockModelPart.IsInvoked);
        }
    }
}
