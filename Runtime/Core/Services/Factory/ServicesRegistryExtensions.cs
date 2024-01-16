namespace Unibrics.Core.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class ServicesRegistryExtensions
    {
        public static IFromBinding Add(this IServicesRegistry registry, params Type[] types)
        {
            var binding = new ServiceDescriptorBuilder(types);
            registry.Add(binding.Descriptor);
            return binding;
        }

        public static IFromBinding<TFrom> Add<TFrom>(this IServicesRegistry registry)
        {
            var binding = new TypedServiceDescriptorBuilder<TFrom>();
            registry.Add(binding.Descriptor);
            return binding;
        }

        public static IFromBinding Add<TFrom1, TFrom2>(this IServicesRegistry registry)
        {
            var binding = new ServiceDescriptorBuilder(new[] { typeof(TFrom1), typeof(TFrom2) });
            registry.Add(binding.Descriptor);
            return binding;
        }

        public static IFromBinding Add<TFrom1, TFrom2, TFrom3>(this IServicesRegistry registry)
        {
            var binding = new ServiceDescriptorBuilder(new[] { typeof(TFrom1), typeof(TFrom2), typeof(TFrom3) });
            registry.Add(binding.Descriptor);
            return binding;
        }
        
        public static IFromBinding<TFrom> Rebind<TFrom>(this IServicesRegistry registry)
        {
            var oldBinding = registry.Get(desc => desc.InterfaceTypes.Contains(typeof(TFrom)));
            if (oldBinding == null)
            {
                throw new ServiceValidationException($"Could not rebind type {typeof(TFrom)}, because it was not bound previously");
            }
            
            oldBinding.RemoveInterfaceType(typeof(TFrom));
            if (oldBinding.InterfaceTypes.Length == 0)
            {
                registry.Drop(oldBinding);
            }
            
            var binding = new TypedServiceDescriptorBuilder<TFrom>();
            registry.Add(binding.Descriptor);
            return binding;
        }
    }
}