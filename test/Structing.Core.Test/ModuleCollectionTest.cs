using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Structing.Core.Test
{
    [TestClass]
    public class ModuleCollectionTest
    {
        
        class NullModuleInfo:Dictionary<string,object>,IModuleInfo
        {

        }
        [TestMethod]
        public void GivenAnyModuelEntries_OrderIsAvg()
        {
            var count = 10;
            var coll = new ModuleCollection();
            var info = new NullModuleInfo();
            for (int i = 0; i < count; i++)
            {
                coll.Add(new ValueModuelEntry { Order = i ,Info=info});
            }
            var order = Enumerable.Range(0, count).Average();
            Assert.AreEqual((int)order, coll.Order);
            Assert.ThrowsException<NotSupportedException>(() => coll.GetModuleInfo(null));
        }
        [TestMethod]
        public void NothingElement_OrderMustZero()
        {
            Assert.AreEqual(0, new ValueModuelEntry().Order);
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

            void Check(Func<ValueModuelEntry,bool> prop,string propertyName)
            {
                for (int i = 0; i < coll.Count; i++)
                {
                    var val = (ValueModuelEntry)coll[i];
                    Assert.IsTrue(prop(val),$"The {i} {propertyName} is not true");
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

            await coll.StartAsync(null);
            Check(x => x.IsStartAsync, nameof(ValueModuelEntry.IsStartAsync));

            await coll.StopAsync(null);
            Check(x => x.IsStopAsync, nameof(ValueModuelEntry.IsStopAsync));

        }
    }
}
