namespace Unibrics.Core.Services
{
    using System;

    public class ServiceDescriptorBuilder : IFromBinding, IToBinding
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

        public IToBinding To<TTo>()
        {
            Descriptor.ImplementationType = typeof(TTo);
            return this;
        }

        public IToBinding To<TTo>(TTo toObject)
        {
            Descriptor.ImplementationObject = toObject;
            return this;
        }
    }

    public class TypedServiceDescriptorBuilder<T> : ServiceDescriptorBuilder, IFromBinding<T>
    {
        public TypedServiceDescriptorBuilder() : base(new []{typeof(T)})
        {
        }

        public new IToBinding To<TTo>(TTo toObject) where TTo : T
        {
            Descriptor.ImplementationObject = toObject;
            return this;
        }
        
        public new IToBinding To<TTo>() where TTo : T
        {
            Descriptor.ImplementationType = typeof(TTo);
            return this;
        }
    }
}