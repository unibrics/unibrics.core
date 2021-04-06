namespace Unibrics.Core.Services
{
    using System;

    public interface IServicesRegistry
    {
        void Add(ServiceDescriptor descriptor);
    }

    public static class ServicesRegistryExtensions
    {
        public static void AddSingleton<TInterface, TImplementation>(this IServicesRegistry registry)
            where TImplementation : TInterface
        {
            registry.Add(new ServiceDescriptor(typeof(TInterface), ServiceScope.Singleton, typeof(TImplementation)));
        }
        
        public static void AddSingleton<TImplementation>(this IServicesRegistry registry, params Type[] interfaces)
        {
            registry.Add(new ServiceDescriptor(interfaces, ServiceScope.Singleton, typeof(TImplementation)));
        }
    }
}