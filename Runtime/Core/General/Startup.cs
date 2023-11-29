namespace Unibrics.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Config;
    using DI;
    using Launchers;
    using Processing;
    using Services;
    using Tools;
    using UnityEngine;
    using Types = Tools.Types;

    public class Startup
    {
        private readonly IDependencyInjectionService diService;

        private readonly List<string> modulesToExclude;

        private readonly List<IModuleInstaller> installers = new();
        
        private List<IModuleInstallerProcessor> processors;

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
            CreateModuleProcessors();
            
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

        private void CreateModuleProcessors()
        {
            processors = Types.AnnotatedWith<InstallAttribute>()
                .WithParent(typeof(IModuleInstallerProcessor))
                .TypesOnly()
                .CreateInstances<IModuleInstallerProcessor>()
                .ToList();
        }

        private void PrepareModules()
        {
            installers.ForEach(installer =>
            {
                processors.ForEach(processor => processor.Process(installer));

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
                    .Where(type => type.GetCustomAttribute<InstallAttribute>() != null)
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
                .OrderByDescending(launcher => launcher.Priority)
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