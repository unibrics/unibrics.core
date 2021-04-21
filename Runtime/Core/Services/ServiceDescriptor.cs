namespace Unibrics.Core.Services
{
    using System;
    using System.Linq;
    using JetBrains.Annotations;
    using Tools;

    public class ServiceDescriptor
    {
        [NotNull]
        public Type[] InterfaceTypes { get; }

        public ServiceScope Scope { get; set; }

        [CanBeNull]
        public Type ImplementationType { get; set; }

        [CanBeNull]
        public object ImplementationObject { get; set; }
        
        public bool IsConfirmed { get; private set; }

        private Action onDescriptorReady;
        
        public ServiceDescriptor([NotNull] Type[] interfaceTypes)
        {
            InterfaceTypes = interfaceTypes;
        }

        public void OnDescriptorFinalized(Action action)
        {
            onDescriptorReady = action;
        }

        public void Confirm()
        {
            IsConfirmed = true;
            onDescriptorReady?.Invoke();
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
    }

    public enum ServiceScope
    {
        Unset,
        Singleton,
        Transient
    }
}