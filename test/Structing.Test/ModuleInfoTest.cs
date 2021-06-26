using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            Assert.IsTrue(info.ContainsKey(ModuleInfoConst.CultureKey));
            Assert.IsTrue(info.ContainsKey(ModuleInfoConst.NameKey));
            Assert.IsTrue(info.ContainsKey(ModuleInfoConst.VersionKey));
        }
    }
}
