using Microsoft.VisualStudio.TestTools.UnitTesting;
using NullModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Structing.Test
{
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
        
    }
}
