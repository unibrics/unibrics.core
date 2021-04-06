namespace Unibrics.Core
{
    using System;
    using System.Linq;
    using DI;
    using Tools;
    using UnityEngine;
    using Types = Tools.Types;

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
            var installers = Types.AnnotatedWith<InstallAttribute>().WithParent(typeof(IModuleInstaller)).TypesOnly()
                .Select(type => (IModuleInstaller) Activator.CreateInstance(type))
                .OrderByDescending(installer => installer.Priority)
                .ToList();

            installers.ForEach(installer => installer.Install(diService));
            diService.PrepareServices();
        }

        private void Launch()
        {
            var launcherType = Types.AnnotatedWith<InstallAttribute>().WithParent(typeof(AppLauncher)).EnsuredSingle()
                .Type();
            if (launcherType == null)
            {
                return;
            }

            var launcher = diService.InstanceProvider.GetInstance<IAppLauncher>(launcherType);
            launcher.Launch();
        }
    }
}