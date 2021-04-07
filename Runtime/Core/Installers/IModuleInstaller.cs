namespace Unibrics.Core
{
    using System;
    using Launchers;
    using Services;

    interface IModuleInstaller
    {
        Priority Priority { get; }

        Type LauncherType { get; }

        void Install(IServicesRegistry services);
    }

}