using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Structing.Core.Test
{
    [TestClass]
    public class RegisteContextTest
    {
        [TestMethod]
        public void GivenNullInit_MustThrowException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new RegisteContext(null));
        }
        [TestMethod]
        public void GivenArguments_PropertyMustEqualInput()
        {
            var services = new ServiceCollection();
            var map = new Dictionary<string, string>();
            var ctx = new RegisteContext(services, map);
            Assert.AreEqual(services, ctx.Services);
            Assert.AreEqual(map, ctx.Features);

            ctx = new RegisteContext(services);
            Assert.AreEqual(services, ctx.Services);
            Assert.IsNotNull(ctx.Features);
        }
    }
}
