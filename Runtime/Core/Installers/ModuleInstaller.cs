namespace Unibrics.Core
{
    using System;
    using Launchers;
    using Services;

    public abstract class ModuleInstaller : IModuleInstaller
    {
        public virtual Priority Priority => Priority.Simple;

        public virtual Type LauncherType => null;

        public abstract void Install(IServicesRegistry services);
    }
    
    
    public abstract class ModuleInstaller<TLauncher> : ModuleInstaller
    {
        public sealed override Type LauncherType => typeof(TLauncher);
    }
}