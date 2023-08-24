using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Structing.Test
{
    [TestClass]
    public class ModuleCollectionTest
    {

        class NullModuleInfo : Dictionary<string, object>, IModuleInfo
        {

        }
        [TestMethod]
        public void GivenAnyModuelEntries_OrderIsAvg()
        {
            var count = 10;
            var coll = new ModuleCollection();
            for (int i = 0; i < count; i++)
            {
                var m = new ModuleInfo { ["a" + i] = i };
                coll.Add(new ValueModuelEntry { Order = i, Info = m });
            }
            var order = Enumerable.Range(0, count).Average();
            Assert.AreEqual((int)order, coll.Order);
            var mIfo = coll.GetModuleInfo(null);
            for (int i = 0; i < count; i++)
            {
                Assert.AreEqual(i, mIfo["a" + i]);
            }
        }
        [TestMethod]
        public void NothingElement_OrderMustZero()
        {
            Assert.AreEqual(0, new ModuleCollection().Order);
        }
        [TestMethod]
        public void Capacity_Set()
        {
            Assert.AreEqual(10, new ModuleCollection(10).Capacity);
        }
        [TestMethod]
        public void Enumerable_Set()
        {
            var item = new ModuleCollection();
            var coll = new ModuleCollection(new[] { item });
            Assert.AreEqual(1, coll.Count);
            Assert.AreEqual(item, coll[0]);
        }
        [TestMethod]
        public async Task GivenAnyModuleEntries_CallAllMethod_AllMustInvoked()
        {
            var coll = new ModuleCollection();
            for (int i = 0; i < 10; i++)
            {
                coll.Add(new ValueModuelEntry());
            }
            var hasObjectType = coll.HasType(typeof(object));
            Assert.IsFalse(hasObjectType);
            var hashValueEntry = coll.HasType(typeof(ValueModuelEntry));
            Assert.IsTrue(hashValueEntry);

            void Check(Func<ValueModuelEntry, bool> prop, string propertyName)
            {
                for (int i = 0; i < coll.Count; i++)
                {
                    var val = (ValueModuelEntry)coll[i];
                    Assert.IsTrue(prop(val), $"The {i} {propertyName} is not true");
                }
            }

            await coll.AfterReadyAsync(null);
            Check(x => x.IsAfterReadyAsync, nameof(ValueModuelEntry.IsAfterReadyAsync));

            await coll.BeforeReadyAsync(null);
            Check(x => x.IsBeforeReadyAsync, nameof(ValueModuelEntry.IsBeforeReadyAsync));

            await coll.ReadyAsync(null);
            Check(x => x.IsReadyAsync, nameof(ValueModuelEntry.IsReadyAsync));

            coll.ReadyRegister(null);
            Check(x => x.IsReadyRegister, nameof(ValueModuelEntry.IsReadyRegister));

            coll.Register(null);
            Check(x => x.IsRegister, nameof(ValueModuelEntry.IsRegister));

            coll.AfterRegister(null);
            Check(x => x.IsAfterRegister, nameof(ValueModuelEntry.IsAfterRegister));

            await coll.StartAsync(null);
            Check(x => x.IsStartAsync, nameof(ValueModuelEntry.IsStartAsync));

            await coll.StopAsync(null);
            Check(x => x.IsStopAsync, nameof(ValueModuelEntry.IsStopAsync));

        }
    }
}
