using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Structing.Annotations;
using Structing.Core;
using System;
using System.Threading.Tasks;

namespace Structing.Test.Annotations
{
    [TestClass]
    public class ModuleIniterAttributeTest
    {
        class AModuleIniter : IModuleInit
        {
            public static bool IsInvokeAsync { get; set; }
            public Task InvokeAsync(IReadyContext context)
            {
                IsInvokeAsync = true;
                return Task.FromResult(1);
            }
        }
        [TestMethod]
        public async Task GivenNoModuleInitType_MustThrowException()
        {
            await Assert.ThrowsExceptionAsync<InvalidOperationException>(() => new ModuleIniterAttribute().ReadyAsync(null, typeof(object)));
        }
        [TestMethod]
        public async Task ModuleIniter_Ready_MustInvoked()
        {
            var attr = new ModuleIniterAttribute();
            await attr.ReadyAsync(null, typeof(AModuleIniter));

            Assert.IsTrue(AModuleIniter.IsInvokeAsync);
        }
        class NoConstructModuleIniter : IModuleInit
        {
            private NoConstructModuleIniter()
            {

            }

            public Task InvokeAsync(IReadyContext context)
            {
                return Task.FromResult(1);
            }
        }
        [TestMethod]
        public async Task GivenNoConstructType_MustThrowException()
        {
            var attr = new ModuleIniterAttribute();
            var services = new ServiceCollection();
            var provider = services.BuildServiceProvider();
            var ctx = new ReadyContext(provider);
            await Assert.ThrowsExceptionAsync<ArgumentException>(() => attr.ReadyAsync(ctx, typeof(NoConstructModuleIniter)));
        }
        class ServiceA
        {

        }
        class ServiceB
        {

        }
        class InjectConstructModuleIniter : IModuleInit
        {

            public int Index { get; set; }
            public InjectConstructModuleIniter(ServiceA serviceA)
            {
                Index = 1;
            }
            public InjectConstructModuleIniter(ServiceA serviceA, ServiceB serviceB)
            {
                Index = 2;

            }
            public Task InvokeAsync(IReadyContext context)
            {
                return Task.FromResult(1);
            }
        }
        [TestMethod]
        public void SelectConstruct_MulityConstruct_NothingSelected()
        {
            var attr = new ModuleIniterAttribute();
            var services = new ServiceCollection();
            var provider = services.BuildServiceProvider();
            var ctx = new ReadyContext(provider);
            var val = attr.CreateModuleInit(ctx, typeof(InjectConstructModuleIniter));
            Assert.IsNull(val);
        }
        [TestMethod]
        public void SelectConstruct_MulityConstruct_SelectMoreParamtersConstruct()
        {
            var attr = new ModuleIniterAttribute();
            var services = new ServiceCollection();
            services.AddSingleton<ServiceA>();
            services.AddSingleton<ServiceB>();
            var provider = services.BuildServiceProvider();
            var ctx = new ReadyContext(provider);
            var val = attr.CreateModuleInit(ctx, typeof(InjectConstructModuleIniter));
            Assert.IsNotNull(val);
            Assert.IsInstanceOfType(val, typeof(InjectConstructModuleIniter));
            Assert.AreEqual(2, ((InjectConstructModuleIniter)val).Index);

            services = new ServiceCollection();
            services.AddSingleton<ServiceA>();
            provider = services.BuildServiceProvider();
            ctx = new ReadyContext(provider);
            val = attr.CreateModuleInit(ctx, typeof(InjectConstructModuleIniter));
            Assert.IsNotNull(val);
            Assert.IsInstanceOfType(val, typeof(InjectConstructModuleIniter));
            Assert.AreEqual(1, ((InjectConstructModuleIniter)val).Index);
        }
        class TagConstructModuleIniter : IModuleInit
        {

            public int Index { get; set; }
            [ModuleInitConstructor]
            public TagConstructModuleIniter(ServiceA serviceA)
            {
                Index = 1;
            }
            public TagConstructModuleIniter(ServiceA serviceA, ServiceB serviceB)
            {
                Index = 2;

            }
            public Task InvokeAsync(IReadyContext context)
            {
                return Task.FromResult(1);
            }
        }
        [TestMethod]
        public void SelectConstruct_MustSelectedTagConstruct()
        {
            var attr = new ModuleIniterAttribute();
            var services = new ServiceCollection();
            services.AddSingleton<ServiceA>();
            services.AddSingleton<ServiceB>();
            var provider = services.BuildServiceProvider();
            var ctx = new ReadyContext(provider);
            var val = attr.CreateModuleInit(ctx, typeof(TagConstructModuleIniter));
            Assert.IsNotNull(val);
            Assert.IsInstanceOfType(val, typeof(TagConstructModuleIniter));
            Assert.AreEqual(1, ((TagConstructModuleIniter)val).Index);

        }
        class DefaultValueConstructModuleIniter : IModuleInit
        {

            [ModuleInitConstructor]
            public DefaultValueConstructModuleIniter(ServiceA serviceA = null)
            {
            }
            public Task InvokeAsync(IReadyContext context)
            {
                return Task.FromResult(1);
            }
        }
        [TestMethod]
        public void SelectConstruct_MustCreateWithDefaultValue()
        {
            var attr = new ModuleIniterAttribute();
            var services = new ServiceCollection();
            var provider = services.BuildServiceProvider();
            var ctx = new ReadyContext(provider);
            var val = attr.CreateModuleInit(ctx, typeof(DefaultValueConstructModuleIniter));
            Assert.IsNotNull(val);
            Assert.IsInstanceOfType(val, typeof(DefaultValueConstructModuleIniter));
        }
        class TagNoArgConstructModuleIniter : IModuleInit
        {
            public int Index { get; set; }
            [ModuleInitConstructor]
            public TagNoArgConstructModuleIniter()
            {
                Index = 1;
            }

            public TagNoArgConstructModuleIniter(ServiceA serviceA)
            {
                Index = 2;
            }
            public Task InvokeAsync(IReadyContext context)
            {
                return Task.FromResult(1);
            }
        }
        [TestMethod]
        public void SelectConstruct_MustSelectedTagNoArgConstruct()
        {
            var attr = new ModuleIniterAttribute();
            var services = new ServiceCollection();
            services.AddSingleton<ServiceA>();
            var provider = services.BuildServiceProvider();
            var ctx = new ReadyContext(provider);
            var val = attr.CreateModuleInit(ctx, typeof(TagNoArgConstructModuleIniter));
            Assert.IsNotNull(val);
            Assert.IsInstanceOfType(val, typeof(TagNoArgConstructModuleIniter));
            Assert.AreEqual(1, ((TagNoArgConstructModuleIniter)val).Index);
        }
    }
}
