using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Structing;
using Structing.Test;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace Structing.Test
{
    [TestClass]
    public class ModuleEntryExtensionsTest
    {
        [ExcludeFromCodeCoverage]
        class NullModuleRegister : IModuleRegister
        {
            public void AfterRegister(IRegisteContext context)
            {
            }

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
        [ExcludeFromCodeCoverage]
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

            await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => ModuleEntryExtensions.RunAsync((IEnumerable<IModuleEntry>)null));
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => ModuleEntryExtensions.RunAsync((IModuleEntry)null));
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => ModuleEntryExtensions.RunReadyAsync(null, rdCtx));
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => ModuleEntryExtensions.RunReadyAsync(mdRd, null));
            Assert.ThrowsException<ArgumentNullException>(() => ModuleEntryExtensions.RunRegister(null, new ServiceCollection()));
            Assert.ThrowsException<ArgumentNullException>(() => ModuleEntryExtensions.RunRegister(null, rsCtx));
            Assert.ThrowsException<ArgumentNullException>(() => ModuleEntryExtensions.RunRegister(moduleRegister, (ServiceCollection)null));
            Assert.ThrowsException<ArgumentNullException>(() => ModuleEntryExtensions.RunRegister(moduleRegister, (IRegisteContext)null));

        }
        [ExcludeFromCodeCoverage]
        class ValueModule : IModuleEntry
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

            public bool IsAfterRegister { get; set; }
            public void AfterRegister(IRegisteContext context)
            {
                IsAfterRegister = true;
            }

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

            public int Order => 0;

            public Task ReadyAsync(IReadyContext context)
            {
                IsReadyAsync = true;
                return Task.FromResult(1);
            }

            public IModuleInfo GetModuleInfo(IServiceProvider provider)
            {
                return ModuleInfo.FromAssembly(GetType().Assembly);
            }

            public bool IsStartAsync { get; set; }
            public Task StartAsync(IServiceProvider serviceProvider)
            {
                return Task.CompletedTask;
            }

            public bool IsStopAsync { get; set; }
            public Task StopAsync(IServiceProvider serviceProvider)
            {
                return Task.CompletedTask;
            }
        }
        [TestMethod]
        public void RunRegister_ServiceMustBeRegisted()
        {
            var rgs = new ValueModule();
            var ctx = new RegisteContext(new ServiceCollection());
            ModuleEntryExtensions.RunRegister(rgs, ctx);
            Assert.IsTrue(rgs.IsReadyRegister);
            Assert.IsTrue(rgs.IsRegister);
            Assert.IsTrue(rgs.IsAfterRegister);

            ModuleEntryExtensions.RunRegister(rgs, new ServiceCollection());
            Assert.IsTrue(rgs.IsReadyRegister);
            Assert.IsTrue(rgs.IsRegister);
            Assert.IsTrue(rgs.IsAfterRegister);

        }
        [TestMethod]
        public async Task RunReady_ServiceMustBeRegisted()
        {
            var rgs = new ValueModule();
            var sp = new NullServiceProvider();
            var ctx = new ReadyContext(sp);
            await ModuleEntryExtensions.RunReadyAsync(rgs, ctx);
            Assert.IsTrue(rgs.IsBeforeReadyAsync);
            Assert.IsTrue(rgs.IsReadyAsync);
            Assert.IsTrue(rgs.IsAfterReadyAsync);
        }
        [TestMethod]
        public async Task RunAsync_ServiceMustBeRegisted()
        {
            var rgs = new ValueModule();
            var sp = new NullServiceProvider();
            var ctx = new ReadyContext(sp);
            await ModuleEntryExtensions.RunAsync(rgs);
            Assert.IsTrue(rgs.IsBeforeReadyAsync);
            Assert.IsTrue(rgs.IsReadyAsync);
            Assert.IsTrue(rgs.IsAfterReadyAsync);

            Assert.IsTrue(rgs.IsReadyRegister);
            Assert.IsTrue(rgs.IsRegister);
            Assert.IsTrue(rgs.IsAfterRegister);
            Assert.IsFalse(rgs.IsStartAsync);
            Assert.IsFalse(rgs.IsStopAsync);

        }
        [TestMethod]
        public void Run_ServiceMustBeRegisted()
        {
            var rgs = new ValueModule();
            var sp = new NullServiceProvider();
            var ctx = new ReadyContext(sp);
            ModuleEntryExtensions.Run(rgs);
            Assert.IsTrue(rgs.IsBeforeReadyAsync);
            Assert.IsTrue(rgs.IsReadyAsync);
            Assert.IsTrue(rgs.IsAfterReadyAsync);

            Assert.IsTrue(rgs.IsReadyRegister);
            Assert.IsTrue(rgs.IsRegister);
            Assert.IsTrue(rgs.IsAfterRegister);
            Assert.IsFalse(rgs.IsStartAsync);
            Assert.IsFalse(rgs.IsStopAsync);

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
            await ModuleEntryExtensions.RunAsync(entries, services, new ConfigurationRoot(new List<IConfigurationProvider>()));
            Check(entries);
        }
    }
}
