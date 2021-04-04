namespace Unibrics.Core.Services
{
    using System;
    using JetBrains.Annotations;

    public class ServiceDescriptor
    {
        [NotNull]
        public Type[] InterfaceTypes { get; }

        public ServiceScope Scope { get; }

        [CanBeNull]
        public Type ImplementationType { get; }

        [CanBeNull]
        public object ImplementationObject { get; }


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
    }

    public enum ServiceScope
    {
        Singleton,
        Transient
    }
}