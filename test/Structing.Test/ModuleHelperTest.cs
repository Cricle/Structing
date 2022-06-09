using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NullModule;
using Structing.Annotations;
using System.Threading.Tasks;

namespace Structing.Test
{
    [EnableService(ServiceLifetime = ServiceLifetime.Singleton)]
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
            var val = prov.GetService<NullServices>();
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
