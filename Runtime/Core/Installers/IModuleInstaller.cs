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
        public virtual Priority Priority => Priority.Simple;
        
        public abstract void Install(IServicesRegistry services);
    }
}