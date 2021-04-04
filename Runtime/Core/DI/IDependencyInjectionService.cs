namespace Unibrics.Core.DI
{
    using Services;

    public interface IDependencyInjectionService : IServicesRegistry
    {
        IInstanceProvider InstanceProvider { get; }

        void PrepareServices();
    }
}