namespace Unibrics.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DI;
    using Launchers;
    using Tools;
    using UnityEngine;
    using Types = Tools.Types;

    public class Startup
    {
        private readonly IDependencyInjectionService diService;

        private List<IModuleInstaller> installers;

        public Startup(IDependencyInjectionService diService)
        {
            this.diService = diService;
        }

        public void StartSequence()
        {
            PrepareModules();
            LaunchModules();
            LaunchApp();
        }


        private void PrepareModules()
        {
            installers = Types.AnnotatedWith<InstallAttribute>()
                .WithParent(typeof(IModuleInstaller))
                .TypesOnly()
                .CreateInstances<IModuleInstaller>()
                .OrderByDescending(installer => installer.Priority)
                .ToList();

            installers.ForEach(installer => installer.Install(diService));
            diService.PrepareServices();
        }


        private void LaunchModules()
        {
            installers
                .Select(installer => installer.LauncherType)
                .Where(type => type != null)
                .Select(type => diService.InstanceProvider.GetInstance<IModuleLauncher>(type))
                .ToList().ForEach(launcher => launcher.Launch());
        }

        private void LaunchApp()
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