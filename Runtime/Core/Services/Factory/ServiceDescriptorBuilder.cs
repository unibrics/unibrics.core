namespace Unibrics.Core.Services
{
    using System;

    public class ServiceDescriptorBuilder : IFromBinding, IToTypeBinding, IToInstanceBinding
    {
        public ServiceDescriptor Descriptor { get; }

        public ServiceDescriptorBuilder(Type[] interfaceTypes)
        {
            Descriptor = new ServiceDescriptor(interfaceTypes);
        }

        public void AsSingleton()
        {
            Descriptor.Scope = ServiceScope.Singleton;
        }

        public void AsTransient()
        {
            Descriptor.Scope = ServiceScope.Transient;
        }

        public IToTypeBinding ImplementedBy<TTo>()
        {
            Descriptor.ImplementationType = typeof(TTo);
            return this;
        }

        public IToTypeBinding ImplementedBy(Type type)
        {
            Descriptor.ImplementationType = type;
            return this;
        }

        public IToInstanceBinding ImplementedByInstance<TTo>(TTo toObject)
        {
            Descriptor.ImplementationObject = toObject;
            return this;
        }

        public IToInstanceBinding QueueInstanceForInjection()
        {
            Descriptor.QueueForInjection = true;
            return this;
        }
    }

    public class TypedServiceDescriptorBuilder<T> : ServiceDescriptorBuilder, IFromBinding<T>
    {
        public TypedServiceDescriptorBuilder() : base(new[] {typeof(T)})
        {
        }

        new public IToInstanceBinding ImplementedByInstance<TTo>(TTo toObject) where TTo : T
        {
            Descriptor.ImplementationObject = toObject;
            return this;
        }
        
        new public IToTypeBinding ImplementedBy<TTo>() where TTo : T
        {
            Descriptor.ImplementationType = typeof(TTo);
            return this;
        }
        
        new public IToTypeBinding ImplementedBy(Type type)
        {
            Descriptor.ImplementationType = type;
            return this;
        }
    }
}