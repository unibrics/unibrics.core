namespace Unibrics.Core
{
    using System;
    using Config;
    using DI.Environment;
    using Launchers;
    using Services;

    public interface IModuleInstaller
    {
        Priority Priority { get; }

        Type LauncherType { get; }
        
        void Prepare(IAppSettings settings, IEnvironmentChecker environmentChecker);

        void Install(IServicesRegistry services);
    }

}