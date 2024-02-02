namespace Unibrics.Core.Services
{
    using System;

    public interface IServicesRegistry
    {
        void Add(ServiceDescriptor descriptor);

        internal protected ServiceDescriptor Get(Predicate<ServiceDescriptor> filter);
        
        internal protected void Drop(ServiceDescriptor descriptor);
    }
}