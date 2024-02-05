namespace Unibrics.Core.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DI;
    using Services;

    class StubDiService : IDependencyInjectionService
    {
        public List<ServiceDescriptor> Descriptors { get; } = new List<ServiceDescriptor>();

        public void Add(ServiceDescriptor descriptor)
        {
            Descriptors.Add(descriptor);
        }

        ServiceDescriptor IServicesRegistry.Get(Predicate<ServiceDescriptor> filter)
        {
            return Descriptors.FirstOrDefault(descriptor => filter(descriptor));
        }

        void IServicesRegistry.Drop(ServiceDescriptor descriptor)
        {
            Descriptors.Remove(descriptor);
        }

        public IInstanceProvider InstanceProvider { get; }

        public IResolver Resolver { get; }

        public void PrepareServices()
        {
            
        }
    }
}