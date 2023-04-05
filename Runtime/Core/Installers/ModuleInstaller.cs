namespace Unibrics.Core
{
    using System;
    using Config;
    using Launchers;
    using Services;

    public abstract class ModuleInstaller : IModuleInstaller
    {
        public virtual Priority Priority => Priority.Simple;

        public virtual Type LauncherType => null;

        protected IAppSettings AppSettings { get; private set; }

        public virtual string Id => "undefined";

        public void Prepare(IAppSettings settings)
        {
            AppSettings = settings;
        }

        public abstract void Install(IServicesRegistry services);
    }
    
    
    public abstract class ModuleInstaller<TLauncher> : ModuleInstaller
    {
        public sealed override Type LauncherType => typeof(TLauncher);
    }
}