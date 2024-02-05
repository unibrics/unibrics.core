namespace Unibrics.Core.DI
{
    using Services;

    public interface IDependencyInjectionService : IServicesRegistry
    {
        IInstanceProvider InstanceProvider { get; }
        
        IResolver Resolver { get; }

        void PrepareServices();
    }
}