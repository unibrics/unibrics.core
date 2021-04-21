namespace Unibrics.Core.Services
{
    using DI;

    public interface IServicesRegistry
    {
        void Add(ServiceDescriptor descriptor);
        
        void Validate();
        
        IResolver Resolver { get; }
        
        IInstanceProvider InstanceProvider { get; }
    }
}