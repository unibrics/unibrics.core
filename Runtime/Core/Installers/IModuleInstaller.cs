namespace Unibrics.Core
{
    using Services;

    interface IModuleInstaller
    {
        Priority Priority { get; }
        void Install(IServicesRegistry services);
    }

    public abstract class ModuleInstaller : IModuleInstaller
    {
        public abstract Priority Priority { get; }
        
        public abstract void Install(IServicesRegistry services);
    }
}