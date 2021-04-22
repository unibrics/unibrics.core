namespace Unibrics.Core.Services
{
    using DI;

    public interface IServicesRegistry
    {
        void Add(ServiceDescriptor descriptor);
        
        IInstanceProvider InstanceProvider { get; }
    }
}