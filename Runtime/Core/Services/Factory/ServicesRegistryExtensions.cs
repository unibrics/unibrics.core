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
    }
}