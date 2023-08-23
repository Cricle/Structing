using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NullModule;
using Structing.Core;
using System.Reflection;
using System.Threading.Tasks;

namespace Structing.Test
{
    [TestClass]
    public class AutoModuleEntityTest
    {
        public class ValueAutoModuleEntity : AutoModuleEntry
        {
            protected internal override Assembly GetAssembly()
            {
                return typeof(NullIniter).Assembly;
            }
        }
        [TestMethod]
        public void AssemblyMustFunReturn()
        {
            Assert.AreEqual(typeof(NullIniter).Assembly, new ValueAutoModuleEntity().GetAssembly());
        }
        [TestMethod]
        [DataRow(true)]
        [DataRow(false)]
        public async Task AutoModule(bool parallel)
        {
            var moduel = new ValueAutoModuleEntity { Parallel = parallel };
            var info = moduel.GetModuleInfo(null);
            Assert.IsNotNull(info);

            var services = new ServiceCollection();
            var ctx = new RegisteContext(services);
            moduel.ReadyRegister(ctx);
            moduel.Register(ctx);

            await moduel.BeforeReadyAsync(null);
            await moduel.ReadyAsync(null);
            await moduel.AfterReadyAsync(null);

            await moduel.StartAsync(null);
            await moduel.StopAsync(null);
            await moduel.CloseAsync(null);

        }
    }
}
