namespace Unibrics.Core.Services
{
    using System;
    using System.Collections.Generic;

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

    }
}