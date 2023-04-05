namespace Unibrics.Core
{
    using System;
    using Config;
    using Launchers;
    using Services;

    interface IModuleInstaller
    {
        Priority Priority { get; }

        Type LauncherType { get; }
        
        void Prepare(IAppSettings settings);

        void Install(IServicesRegistry services);
    }

}