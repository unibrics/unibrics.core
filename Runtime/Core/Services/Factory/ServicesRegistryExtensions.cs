namespace Unibrics.Core.Services
{
    using System;
    using System.Collections.Generic;

    public static class ServicesRegistryExtensions
    {
        public static IFromBinding Bind(this IServicesRegistry registry, params Type[] types)
        {
            var binding = new ServiceDescriptorBuilder(types);
            registry.Add(binding.Descriptor);
            return binding;
        }

        public static IFromBinding<TFrom> Bind<TFrom>(this IServicesRegistry registry)
        {
            var binding = new TypedServiceDescriptorBuilder<TFrom>();
            registry.Add(binding.Descriptor);
            return binding;
        }



        public static void AddSingleton<TInterface, TImplementation>(this IServicesRegistry registry)
            where TImplementation : TInterface
        {
            registry.Add(new ServiceDescriptor(typeof(TInterface), ServiceScope.Singleton, typeof(TImplementation)));
        }
        
        public static void AddSingleton<TInterface>(this IServicesRegistry registry, object value)
        {
            registry.Add(new ServiceDescriptor(typeof(TInterface), ServiceScope.Singleton, implementationObject:value));
        }
        
        public static void AddSingleton<TImplementation>(this IServicesRegistry registry, params Type[] interfaces)
        {
            registry.Add(new ServiceDescriptor(interfaces, ServiceScope.Singleton, typeof(TImplementation)));
        }
        
        public static void AddSingleton(this IServicesRegistry registry, List<Type> interfaces, Type implementation)
        {
            registry.Add(new ServiceDescriptor(interfaces.ToArray(), ServiceScope.Singleton, implementation));
        }
    }
}