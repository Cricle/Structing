using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NullModule;
using Structing.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Structing.Test
{
    [TestClass]
    public class AutoModuleEntityTest
    {
        public class ValueAutoModuleEntity : AutoModuleEntry
        {
            protected override Assembly GetAssembly()
            {
                return typeof(NullIniter).Assembly;
            }
        }
        [TestMethod]
        [DataRow(true)]
        [DataRow(false)]
        public async Task AutoModule(bool parallel)
        {
            var moduel = new ValueAutoModuleEntity { Parallel= parallel };
            var info=moduel.GetModuleInfo(null);
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
