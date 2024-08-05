namespace Unibrics.Core.Services
{
    using System;
    using System.Linq;

    public static class ServiceRegistryRebindExtensions
    {
        
        public static IFromBinding<TFrom> Rebind<TFrom>(this IServicesRegistry registry)
        {
            var oldBinding = registry.Get(desc => desc.InterfaceTypes.Contains(typeof(TFrom)));
            if (oldBinding == null)
            {
                throw new ServiceValidationException(
                    $"Could not rebind type {typeof(TFrom)}, because it was not bound previously");
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

        public static IFromBinding Rebind<TFrom1, TFrom2>(this IServicesRegistry registry)
        {
            return Rebind(registry, typeof(TFrom1), typeof(TFrom2));
        }

        public static IFromBinding Rebind<TFrom1, TFrom2, TFrom3>(this IServicesRegistry registry)
        {
            return Rebind(registry, typeof(TFrom1), typeof(TFrom2), typeof(TFrom3));
        }

        public static IFromBinding Rebind(this IServicesRegistry registry, params Type[] types)
        {
            registry.Unbind(types);

            var binding = new ServiceDescriptorBuilder(types);
            registry.Add(binding.Descriptor);
            return binding;
        }
    }
}