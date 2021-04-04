namespace Unibrics.Core
{
    using System;
    using System.Linq;
    using DI;
    using Tools;

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
    }
}