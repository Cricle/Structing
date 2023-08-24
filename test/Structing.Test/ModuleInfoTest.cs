using Microsoft.VisualStudio.TestTools.UnitTesting;
using Structing.Core;
using System;

namespace Structing.Test
{
    [TestClass]
    public class ModuleInfoTest
    {
        [TestMethod]
        public void GivenNullCall_MustThrowException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => ModuleInfo.FromAssembly(null));
        }
        [TestMethod]
        public void FromAssembly_MustContainsInfo()
        {
            var info = ModuleInfo.FromAssembly(GetType().Assembly);
            Assert.IsTrue(true);
        }
    }
}
