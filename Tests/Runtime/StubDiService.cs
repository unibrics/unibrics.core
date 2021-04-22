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

        public IInstanceProvider InstanceProvider { get; }
        public void PrepareServices()
        {
            
        }
    }
}