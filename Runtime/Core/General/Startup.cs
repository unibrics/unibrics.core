namespace Unibrics.Core
{
    using System;
    using System.Linq;
    using DI;
    using Tools;
    using UnityEngine;

    public class Startup
    {
        private readonly IDependencyInjectionService diService;

        public Startup(IDependencyInjectionService diService)
        {
            this.diService = diService;
        }

        public void StartApp()
        {
            PrepareModules();
            Launch();
        }

        private void PrepareModules()
        {
            var installers = AssemblyExtensions.GetTypesWithAttribute<InstallAttribute, IModuleInstaller>()
                .Select(type => (IModuleInstaller) Activator.CreateInstance(type))
                .OrderByDescending(installer => installer.Priority)
                .ToList();
            
            installers.ForEach(installer => installer.Install(diService));
            diService.PrepareServices();
        }
        
        private void Launch()
        {
            var launcherType = AssemblyExtensions.GetSingleTypeWithAttribute<InstallAttribute, AppLauncher>();
            if (launcherType == null)
            {
                return;
            }

            var launcher = diService.InstanceProvider.GetInstance<IAppLauncher>(launcherType);
            launcher.Launch();
        }
    }
}