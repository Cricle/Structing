using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Structing.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Structing.Core.Test;
using Microsoft.Extensions.Configuration;

namespace Structing.Test
{
    [TestClass]
    public class ModuleEntryExtensionsTest
    {
        class NullModuleRegister : IModuleRegister
        {
            public void ReadyRegister(IRegisteContext context)
            {
            }

            public void Register(IRegisteContext context)
            {
            }
        }
        class NullServiceProvider : IServiceProvider
        {
            public object GetService(Type serviceType)
            {
                return null;
            }
        }
        class NullModuelReady : IModuleReady
        {
            public Task AfterReadyAsync(IReadyContext context)
            {
                return Task.FromResult(1);
            }

            public Task BeforeReadyAsync(IReadyContext context)
            {
                return Task.FromResult(1);
            }

            public Task ReadyAsync(IReadyContext context)
            {
                return Task.FromResult(1);
            }
        }
        [TestMethod]
        public async Task GivenNullCall_MustThrowException()
        {
            var moduleRegister = new NullModuleRegister();
            var rdCtx = new ReadyContext(new NullServiceProvider());
            var rsCtx = new RegisteContext(new ServiceCollection());
            var mdRd = new NullModuelReady();

            await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => ModuleEntryExtensions.RunAsync(null));
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => ModuleEntryExtensions.RunReadyAsync(null,rdCtx));
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => ModuleEntryExtensions.RunReadyAsync(mdRd,null));
            Assert.ThrowsException<ArgumentNullException>(() => ModuleEntryExtensions.RunRegister(null, new ServiceCollection()));
            Assert.ThrowsException<ArgumentNullException>(() => ModuleEntryExtensions.RunRegister(null, rsCtx));
            Assert.ThrowsException<ArgumentNullException>(() => ModuleEntryExtensions.RunRegister(moduleRegister, (ServiceCollection)null));
            Assert.ThrowsException<ArgumentNullException>(() => ModuleEntryExtensions.RunRegister(moduleRegister, (IRegisteContext)null));

        }
        class ValueModuleRegister : IModuleRegister
        {
            public bool IsReadyRegister { get; set; }
            public void ReadyRegister(IRegisteContext context)
            {
                IsReadyRegister = true;
            }
            public bool IsRegister { get; set; }
            public void Register(IRegisteContext context)
            {
                IsRegister = true;
            }
        }
        [TestMethod]
        public void RunRegister_ServiceMustBeRegisted()
        {
            var rgs = new ValueModuleRegister();
            var ctx = new RegisteContext(new ServiceCollection());
            ModuleEntryExtensions.RunRegister(rgs,ctx);
            Assert.IsTrue(rgs.IsReadyRegister);
            Assert.IsTrue(rgs.IsRegister);

            ModuleEntryExtensions.RunRegister(rgs, new ServiceCollection());
            Assert.IsTrue(rgs.IsReadyRegister);
            Assert.IsTrue(rgs.IsRegister);
        }
        class ValueModuelReady : IModuleReady
        {
            public bool IsAfterReadyAsync { get; set; }
            public Task AfterReadyAsync(IReadyContext context)
            {
                IsAfterReadyAsync = true;
                return Task.FromResult(1);
            }

            public bool IsBeforeReadyAsync { get; set; }
            public Task BeforeReadyAsync(IReadyContext context)
            {
                IsBeforeReadyAsync = true;
                return Task.FromResult(1);
            }

            public bool IsReadyAsync { get; set; }
            public Task ReadyAsync(IReadyContext context)
            {
                IsReadyAsync = true;
                return Task.FromResult(1);
            }
        }
        [TestMethod]
        public async Task RunReady_ServiceMustBeRegisted()
        {
            var rgs = new ValueModuelReady();
            var sp = new NullServiceProvider();
            var ctx = new ReadyContext(sp);
            await ModuleEntryExtensions.RunReadyAsync(rgs, ctx);
            Assert.IsTrue(rgs.IsBeforeReadyAsync);
            Assert.IsTrue(rgs.IsReadyAsync);
            Assert.IsTrue(rgs.IsAfterReadyAsync);
        }
        [TestMethod]
        public async Task Run()
        {
            var entries = new List<IModuleEntry>();
            for (int i = 0; i < 10; i++)
            {
                entries.Add(new ValueModuelEntry());
            }
            void Check(IEnumerable<IModuleEntry> modules)
            {
                foreach (ValueModuelEntry item in modules)
                {
                    Assert.IsTrue(item.IsRegister);
                    Assert.IsTrue(item.IsBeforeReadyAsync);
                    Assert.IsTrue(item.IsReadyAsync);
                    Assert.IsTrue(item.IsAfterReadyAsync);
                }
            }
            await ModuleEntryExtensions.RunAsync(entries);
            Check(entries);

            entries = new List<IModuleEntry>();
            for (int i = 0; i < 10; i++)
            {
                entries.Add(new ValueModuelEntry());
            }
            var services = new ServiceCollection();
            await ModuleEntryExtensions.RunAsync(entries, services);
            Check(entries);

            entries = new List<IModuleEntry>();
            for (int i = 0; i < 10; i++)
            {
                entries.Add(new ValueModuelEntry());
            }
            await ModuleEntryExtensions.RunAsync(entries, services,new ConfigurationRoot(new List<IConfigurationProvider>()));
            Check(entries);
        }
    }
}
