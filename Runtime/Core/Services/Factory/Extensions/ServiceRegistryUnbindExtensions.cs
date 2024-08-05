namespace Unibrics.Core.Services
{
    using System;
    using System.Linq;

    public static class ServiceRegistryUnbindExtensions
    {
        
        public static void Unbind<TType>(this IServicesRegistry registry)
        {
            var type = typeof(TType);
            var oldBinding = registry.Get(desc => desc.InterfaceTypes.Contains(type));
            if (oldBinding == null)
            {
                throw new ServiceValidationException(
                    $"Could not find a binding for {type}" +
                    $" may be they were bound to different implementations");
            }

            oldBinding.RemoveInterfaceType(type);

            if (oldBinding.InterfaceTypes.Length == 0)
            {
                registry.Drop(oldBinding);
            }
        }
        
        public static void Unbind<TType1, TType2>(this IServicesRegistry registry)
        {
            Unbind(registry, typeof(TType1), typeof(TType2));
        }

        public static void Unbind<TType1, TType2, TType3>(this IServicesRegistry registry)
        {
            Unbind(registry, typeof(TType1), typeof(TType2), typeof(TType3));
        }

        public static void Unbind(this IServicesRegistry registry, params Type[] types)
        {
            var oldBinding = registry.Get(desc => types.All(type => desc.InterfaceTypes.Contains(type)));
            if (oldBinding == null)
            {
                throw new ServiceValidationException(
                    $"Could not find a binding that binds following types: {string.Join<Type>(",", types)}," +
                    $" may be they were bound to different implementations");
            }

            foreach (var type in types)
            {
                oldBinding.RemoveInterfaceType(type);
            }

            if (oldBinding.InterfaceTypes.Length == 0)
            {
                registry.Drop(oldBinding);
            }

        }
    }
}