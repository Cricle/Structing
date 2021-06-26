using Microsoft.VisualStudio.TestTools.UnitTesting;
using NullModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Structing.Annotations;

namespace Structing.Test
{
    [EnableService(ServiceLifetime= ServiceLifetime.Singleton)]
    internal class ServiceH
    {

    }
    [TestClass]
    public class ModuleHelperTest
    {
        [TestMethod]
        public async Task RunAssembly_ServiceMustBeAdded()
        {
            var prov = await ModuleHelper.RunAssemblyAsync(typeof(NullIniter).Assembly);
            var val=prov.GetService<NullServices>();
            Assert.IsNotNull(val);
        }
#if NET5_0
        [TestMethod]
        public async Task RunLocalAssembly_ServiceMustBeAdded()
        {
            var prov = await ModuleHelper.RunAssemblyAsync();
            var val = prov.GetService<ServiceH>();
            Assert.IsNotNull(val);
        }
#endif
    }
}
