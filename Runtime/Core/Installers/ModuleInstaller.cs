namespace Unibrics.Core
{
    using System;
    using Config;
    using DI.Environment;
    using Launchers;
    using Services;

    public abstract class ModuleInstaller : IModuleInstaller
    {
        public virtual Priority Priority => Priority.Simple;

        public virtual Type LauncherType => null;

        protected IAppSettings AppSettings { get; private set; }
        
        protected IEnvironmentChecker EnvironmentChecker { get; private set; }

        public void Prepare(IAppSettings settings, IEnvironmentChecker environmentChecker)
        {
            EnvironmentChecker = environmentChecker;
            AppSettings = settings;
        }

        public abstract void Install(IServicesRegistry services);
    }
    
    
    public abstract class ModuleInstaller<TLauncher> : ModuleInstaller
    {
        public sealed override Type LauncherType => typeof(TLauncher);
    }
}