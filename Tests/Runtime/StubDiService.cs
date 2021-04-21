namespace Unibrics.Core.Tests
{
    using System.Collections.Generic;
    using DI;
    using Services;

    class StubDiService : IDependencyInjectionService
    {
        public List<ServiceDescriptor> Descriptors { get; } = new List<ServiceDescriptor>();

        public void Add(ServiceDescriptor descriptor)
        {
            Descriptors.Add(descriptor);
        }

        public IResolver Resolver { get; }

        public IInstanceProvider InstanceProvider { get; }
        
        public void Validate()
        {
            
        }
    }
}