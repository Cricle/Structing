using Microsoft.VisualStudio.TestTools.UnitTesting;
using Structing.Core;
using System;

namespace Structing.Test
{
    [TestClass]
    public class ThisModuleEntryTest
    {
        [TestMethod]
        public void GivenNullInit_MustThrowException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new ThisModuleEntry(null));
            Assert.ThrowsException<ArgumentNullException>(() => new ThisModuleEntry(null, null));
        }
        [TestMethod]
        public void GivenValueInit_PropertyValueMustEqualInput()
        {
            var ass = typeof(ThisModuleEntryTest).Assembly;
            var info = new ModuleInfo();
            Func<IServiceProvider, IModuleInfo> func = x => info;
            var entry = new ThisModuleEntry(ass, func);
            Assert.AreEqual(ass, entry.Assembly);
            Assert.AreEqual(func, entry.ModuleInfoFactory);

            Assert.AreEqual(info, entry.GetModuleInfo(null));

            entry = new ThisModuleEntry(ass);
            Assert.AreEqual(ass, entry.Assembly);
            Assert.IsNull(entry.ModuleInfoFactory);
            Assert.IsNotNull(entry.GetModuleInfo(null));
        }
    }
}
