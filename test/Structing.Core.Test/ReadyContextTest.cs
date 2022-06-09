using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Structing.Core.Test
{
    [TestClass]
    public class ReadyContextTest
    {
        [TestMethod]
        public void GivenNullInit_MustThrowExcpetion()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new ReadyContext(null));
            Assert.ThrowsException<ArgumentNullException>(() => new ReadyContext(null, null, null));
        }

        [TestMethod]
        public void GivenArguments_PropertyValueMustInput()
        {
            var config = new ConfigurationBuilder().Build();
            var provider = new ServiceCollection()
                .AddSingleton<IConfiguration>(config)
                .BuildServiceProvider();
            var map = new Dictionary<string, string>();
            var ctx = new ReadyContext(provider, map);
            Assert.AreEqual(provider, ctx.Provider);
            Assert.AreEqual(config, ctx.Configuration);
            Assert.AreEqual(map, ctx.Features);

            ctx = new ReadyContext(provider);
            Assert.AreEqual(provider, ctx.Provider);
            Assert.AreEqual(config, ctx.Configuration);
            Assert.IsNotNull(ctx.Features);

            ctx = new ReadyContext(provider, config, map);
            Assert.AreEqual(provider, ctx.Provider);
            Assert.AreEqual(config, ctx.Configuration);
            Assert.AreEqual(map, ctx.Features);
        }
        [TestMethod]
        public void GetService_MustGetFromProvider()
        {
            var config = new ConfigurationBuilder().Build();
            var provider = new ServiceCollection()
                .AddSingleton<IConfiguration>(config)
                .BuildServiceProvider();
            var ctx = new ReadyContext(provider);
            var cfg = ctx.GetService(typeof(IConfiguration));
            Assert.AreEqual(config, cfg);
        }
    }
}
