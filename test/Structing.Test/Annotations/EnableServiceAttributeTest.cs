using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Structing.Annotations;
using Structing.Core;
using Microsoft.Extensions.DependencyInjection;

namespace Structing.Test.Annotations
{
    [TestClass]
    public class EnableServiceAttributeTest
    {
        interface IA
        {

        }
        class A:IA
        {

        }
        private IRegisteContext MakeRegisterContext()
        {
            return new RegisteContext(new ServiceCollection());
        }
        [TestMethod]
        public void GivenNothing_RegistMustInputType()
        {
            var attr = new EnableServiceAttribute
            {
                ServiceLifetime = ServiceLifetime.Singleton
            };
            var ctx = MakeRegisterContext();
            attr.Register(ctx, typeof(object));
            var desc = ctx.Services[0];
            Assert.AreEqual(typeof(object), desc.ServiceType);
            Assert.AreEqual(typeof(object), desc.ImplementationType);
            Assert.AreEqual(ServiceLifetime.Singleton, desc.Lifetime);
        }
        [TestMethod]
        public void GivenImplementType_RegistSplited()
        {
            var attr = new EnableServiceAttribute
            {
                ServiceLifetime = ServiceLifetime.Transient,
                ImplementType=typeof(A)
            };
            var ctx = MakeRegisterContext();
            attr.Register(ctx, typeof(IA));
            var desc = ctx.Services[0];
            Assert.AreEqual(typeof(IA), desc.ServiceType);
            Assert.AreEqual(typeof(A), desc.ImplementationType);
            Assert.AreEqual(ServiceLifetime.Transient, desc.Lifetime);
        }
        [TestMethod]
        public void GivenAllType_RegistSplited()
        {
            var attr = new EnableServiceAttribute
            {
                ServiceLifetime = ServiceLifetime.Scoped,
                ImplementType = typeof(A),
                ServiceType=typeof(IA)
            };
            var ctx = MakeRegisterContext();
            attr.Register(ctx, typeof(object));
            var desc = ctx.Services[0];
            Assert.AreEqual(typeof(IA), desc.ServiceType);
            Assert.AreEqual(typeof(A), desc.ImplementationType);
            Assert.AreEqual(ServiceLifetime.Scoped, desc.Lifetime);
        }
    }
}
