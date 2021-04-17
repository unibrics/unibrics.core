namespace Unibrics.Core.Services
{
    using System;
    using System.Linq;
    using JetBrains.Annotations;
    using Tools;

    public class ServiceDescriptor
    {
        [NotNull] public Type[] InterfaceTypes { get; }

        public ServiceScope Scope { get; set; }

        [CanBeNull] public Type ImplementationType { get; set; }

        [CanBeNull] public object ImplementationObject { get; set; }

        public ServiceDescriptor([NotNull] Type[] interfaceTypes)
        {
            InterfaceTypes = interfaceTypes;
        }

        public ServiceDescriptor([NotNull] Type[] interfaceTypes, ServiceScope scope,
            [CanBeNull] Type implementationType = null, [CanBeNull] object implementationObject = null)
        {
            InterfaceTypes = interfaceTypes;
            Scope = scope;
            ImplementationType = implementationType;
            ImplementationObject = implementationObject;
        }

        public ServiceDescriptor(Type interfaceType, ServiceScope scope, [CanBeNull] Type implementationType = null,
            [CanBeNull] object implementationObject = null)
        {
            InterfaceTypes = new[] {interfaceType};
            Scope = scope;
            ImplementationType = implementationType;
            ImplementationObject = implementationObject;
        }

        public void Validate()
        {
            if (ImplementationObject == null && ImplementationType == null)
            {
                throw new ServiceValidationException($"You forget to provide implementation type" +
                                                            $" or object for {BindingName()}");
            }

            if (Scope == ServiceScope.Unset)
            {
                throw new ServiceValidationException($"Scope was not set for {BindingName()}");
            }

            string BindingName() => $"[{string.Join(",", InterfaceTypes.Select(type => type.Name))}]";
        }
    }

    public enum ServiceScope
    {
        Unset,
        Singleton,
        Transient
    }
}