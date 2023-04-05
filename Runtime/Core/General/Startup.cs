namespace Unibrics.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
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

        private readonly List<IModuleInstaller> installers = new();

        private IAppSettings settings;

        public Startup(IDependencyInjectionService diService, List<string> modulesToExclude)
        {
            this.modulesToExclude = modulesToExclude;
            this.diService = diService;
        }

        public void Prepare()
        {
            // scan entire app for installers and searchable types,
            // skip if needed and cache for further use
            ScanAppTypes();
            
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
            installers.ForEach(installer =>
            {
                installer.Prepare(settings);
                installer.Install(diService);
            });
            diService.PrepareServices();
        }
        
        private void ScanAppTypes()
        {
            var types = new List<Type>();
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies()
                .Where(assembly => assembly.GetCustomAttribute<UnibricsDiscoverableAttribute>() != null))
            {
                var assemblyTypes = assembly.GetTypes();
                var idAttribute = assembly.GetCustomAttribute<UnibricsModuleIdAttribute>();
                if (idAttribute != null)
                {
                    if (modulesToExclude.Contains(idAttribute.Id))
                    {
                        continue;
                    }
                }
                
                installers
                    .AddRange(assemblyTypes
                    .Where(type => !type.IsAbstract && typeof(IModuleInstaller).IsAssignableFrom(type))
                    .Select(installerType => (IModuleInstaller)Activator.CreateInstance(installerType)));
                
                types.AddRange(assemblyTypes);
            }

            Types.SetSearchableTypes(types);
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
            var launcherType = Types.AnnotatedWith<InstallAttribute>()
                .WithParent(typeof(AppLauncher))
                .EnsuredSingle()
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