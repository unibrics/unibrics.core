namespace Unibrics.Core.Tests
{
    using System;
    using System.Linq;
    using NUnit.Framework;
    using Services;

    [TestFixture]
    public class DiTests
    {
        private StubDiService services;

        private ServiceDescriptor FirstDescriptor => services.Descriptors.First();

        [SetUp]
        public void SetUp()
        {
            services = new StubDiService();
        }

        [Test]
        public void _01ShouldBindSingleTypeToType()
        {
            services.Bind<IFirstInterface>().To<FirstImplementation>().AsSingleton();

            Assert.That(FirstDescriptor.InterfaceTypes.Contains(typeof(IFirstInterface)));
            Assert.That(FirstDescriptor.ImplementationType, Is.EqualTo(typeof(FirstImplementation)));
            
            Assert.That(FirstDescriptor.Scope, Is.EqualTo(ServiceScope.Singleton));
        }

        [Test]
        public void _02ShouldBindMultipleTypesToType()
        {
            services
                .Bind(typeof(IFirstInterface), typeof(ISecondInterface))
                .To<FirstImplementation>().AsSingleton();

            Assert.That(FirstDescriptor.InterfaceTypes.Contains(typeof(IFirstInterface)));
            Assert.That(FirstDescriptor.InterfaceTypes.Contains(typeof(ISecondInterface)));

            Assert.That(FirstDescriptor.ImplementationType, Is.EqualTo(typeof(FirstImplementation)));
        }

        [Test]
        public void _03ShouldBindToInstance()
        {
            var instance = new FirstImplementation();

            services.Bind<IFirstInterface>().To(instance);
            
            Assert.That(FirstDescriptor.ImplementationObject, Is.SameAs(instance));
        }

        [Test]
        public void _04ShouldBindToTransientLifetime()
        {
            services.Bind<IFirstInterface>().To<FirstImplementation>().AsTransient();
            
            Assert.That(FirstDescriptor.Scope, Is.EqualTo(ServiceScope.Transient));
        }

        [Test]
        public void _05ShouldBeValidOnlyAfterLifetimeSet()
        {
            var from = services.Bind<IFirstInterface>();
            Assert.Throws<ServiceValidationException>(FirstDescriptor.Validate);

            var to = from.To<FirstImplementation>();
            Assert.Throws<ServiceValidationException>(FirstDescriptor.Validate);
            
            to.AsSingleton();
            Assert.DoesNotThrow(FirstDescriptor.Validate);
        }
    }
}