namespace Unibrics.Core.Tests
{
    using System;
    using System.Linq;
    using NUnit.Framework;
    using Services;
    using Tools;

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
            services.Add<IFirstInterface>().ImplementedBy<FirstImplementation>().AsSingleton();

            Assert.That(FirstDescriptor.InterfaceTypes.Contains(typeof(IFirstInterface)));
            Assert.That(FirstDescriptor.ImplementationType, Is.EqualTo(typeof(FirstImplementation)));
            
            Assert.That(FirstDescriptor.Scope, Is.EqualTo(ServiceScope.Singleton));
        }

        [Test]
        public void _02ShouldBindMultipleTypesToType()
        {
            services
                .Add(typeof(IFirstInterface), typeof(ISecondInterface))
                .ImplementedBy<FirstImplementation>().AsSingleton();

            Assert.That(FirstDescriptor.InterfaceTypes.Contains(typeof(IFirstInterface)));
            Assert.That(FirstDescriptor.InterfaceTypes.Contains(typeof(ISecondInterface)));

            Assert.That(FirstDescriptor.ImplementationType, Is.EqualTo(typeof(FirstImplementation)));
        }

        [Test]
        public void _03ShouldBindToInstance()
        {
            var instance = new FirstImplementation();

            services.Add<IFirstInterface>().ImplementedByInstance(instance);
            
            Assert.That(FirstDescriptor.ImplementationObject, Is.SameAs(instance));
        }

        [Test]
        public void _04ShouldBindToTransientLifetime()
        {
            services.Add<IFirstInterface>().ImplementedBy<FirstImplementation>().AsTransient();
            
            Assert.That(FirstDescriptor.Scope, Is.EqualTo(ServiceScope.Transient));
        }

        [Test]
        public void _05ShouldBeValidOnlyAfterLifetimeSet()
        {
            var from = services.Add<IFirstInterface>();
            Assert.Throws<ServiceValidationException>(FirstDescriptor.Validate);

            var to = from.ImplementedBy<FirstImplementation>();
            Assert.Throws<ServiceValidationException>(FirstDescriptor.Validate);
            
            to.AsSingleton();
            Assert.DoesNotThrow(FirstDescriptor.Validate);
        }
        
        [Test]
        public void _06ShouldBeValidAfterFromInstanceSet()
        {
            var from = services.Add<IFirstInterface>();
            Assert.Throws<ServiceValidationException>(FirstDescriptor.Validate);

            from.ImplementedByInstance(new FirstImplementation());
            Assert.DoesNotThrow(FirstDescriptor.Validate);
        }

        [Test]
        public void _07RebindMustReplaceSingleBind()
        {
            services.Add<IFirstInterface>().ImplementedBy<FirstImplementation>().AsSingleton();
            services.Rebind<IFirstInterface>().ImplementedBy<SecondImplementation>().AsSingleton();
            
            Assert.That(services.Descriptors.Count, Is.EqualTo(1));
            Assert.That(FirstDescriptor.ImplementationType, Is.EqualTo(typeof(SecondImplementation)));
        }
        
        [Test]
        public void _08RebindMustReplaceMultiBind()
        {
            services.Add<IFirstInterface, ISecondInterface>().ImplementedBy<FirstImplementation>().AsSingleton();
            services.Rebind<IFirstInterface>().ImplementedBy<SecondImplementation>().AsSingleton();

            Assert.That(services.Descriptors.Count, Is.EqualTo(2));
            
            Assert.That(FirstDescriptor.InterfaceTypes.Length, Is.EqualTo(1));
            Assert.That(FirstDescriptor.ImplementationType, Is.EqualTo(typeof(FirstImplementation)));

            var secondDescriptor = services.Descriptors[1];
            Assert.That(secondDescriptor.InterfaceTypes.Length, Is.EqualTo(1));
            Assert.That(secondDescriptor.ImplementationType, Is.EqualTo(typeof(SecondImplementation)));
        }
        
        [Test]
        public void _09RebindMustThrowWhenNothingToRebind()
        {
            Assert.Throws<ServiceValidationException>(() =>
            {
                services.Rebind<IFirstInterface>().ImplementedBy<SecondImplementation>().AsSingleton();
            });
        }
    }
}