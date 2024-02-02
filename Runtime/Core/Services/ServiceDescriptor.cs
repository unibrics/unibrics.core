namespace Unibrics.Core.Services
{
    using System;
    using System.Linq;
    using JetBrains.Annotations;
    using Tools;

    public class ServiceDescriptor
    {
        [NotNull]
        public Type[] InterfaceTypes { get; private set; }

        public ServiceScope Scope { get; set; }

        [CanBeNull]
        public Type ImplementationType { get; set; }

        [CanBeNull]
        public object ImplementationObject { get; set; }
       
        public bool QueueForInjection { get; set; }
       
        public ServiceDescriptor([NotNull] Type[] interfaceTypes)
        {
            InterfaceTypes = interfaceTypes;
        }

        public void Validate()
        {
            if (ImplementationObject == null && ImplementationType == null)
            {
                throw new ServiceValidationException($"You forget to provide implementation type" +
                                                     $" or object for {BindingName()}");
            }

            if (Scope == ServiceScope.Unset && ImplementationType != null)
            {
                throw new ServiceValidationException($"Scope was not set for type binding {BindingName()}");
            }

            string BindingName() => $"[{string.Join(",", InterfaceTypes.Select(type => type.Name))}]";
        }

        public void RemoveInterfaceType(Type type)
        {
            var typesList = InterfaceTypes.ToList();
            typesList.Remove(type);
            InterfaceTypes = typesList.ToArray();
        }
    }

    public enum ServiceScope
    {
        Unset,
        Singleton,
        Transient
    }
}