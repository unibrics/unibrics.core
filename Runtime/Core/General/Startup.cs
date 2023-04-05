namespace Unibrics.Core
{
    using System.Collections.Generic;
    using System.Linq;
    using Config;
    using DI;
    using Launchers;
    using Services;
    using Tools;
    using UnityEngine;
    using Types = Tools.Types;

    public class Startup
    {
        private readonly IDependencyInjectionService diService;

        private readonly List<string> modulesToExclude;

        private List<IModuleInstaller> installers;

        private IAppSettings settings;

        public Startup(IDependencyInjectionService diService, List<string> modulesToExclude)
        {
            this.modulesToExclude = modulesToExclude;
            this.diService = diService;
        }

        public void Prepare()
        {
            LoadAppConfig();
            PrepareModules();
        }

        public void Start()
        {
            LaunchModules();
            LaunchApp();
        }

        private void LoadAppConfig()
        {
            settings = new AppSettingsFactory().LoadAppSettings();
            diService.Add<IAppSettings>().ImplementedByInstance(settings);
        }

        private void PrepareModules()
        {
            installers = Types.AnnotatedWith<InstallAttribute>()
                .WithParent(typeof(IModuleInstaller))
                .TypesOnly()
                .CreateInstances<IModuleInstaller>()
                .Where(installer => !modulesToExclude.Contains(installer.Id))
                .OrderByDescending(installer => installer.Priority)
                .ToList();

            installers.ForEach(installer =>
            {
                installer.Prepare(settings);
                installer.Install(diService);
            });
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