using Unibrics.Core.DI;
using Unibrics.Core.Execution;
using Unibrics.Core.Services;

namespace Unibrics.Core
{
    using System.Collections.Generic;
    using System.Linq;
    using DI;
    using DI.Environment;
    using Execution;
    using Features;
    using Launchers;
    using Services;
    using Unibrics.Utils.Json;
    using Utils;
    using Version;

    [Install]
    class UnibricsCoreInstaller : ModuleInstaller<CoreLauncher>
    {
        public override Priority Priority => Priority.Highest;

        public override void Install(IServicesRegistry services)
        {
            services.Add<IInitializablesRegistry>().ImplementedBy<InitializablesRegistry>().AsSingleton();
            services.Add<IEnvironmentChecker, IEnvironmentSetter>().ImplementedBy<EnvironmentHandler>().AsSingleton();
            services.Add<IFeatureSet, IInitializable>().ImplementedBy<FeatureSet>().AsSingleton();
            services.Add<IFeatureSuspender>().ImplementedBy<FeatureSuspender>().AsSingleton();
            services.Add<IVersionProvider>().ImplementedBy<AppVersionProvider>().AsSingleton();
            services.Add<IDeviceIdProvider>().ImplementedBy<DeviceIdProvider>().AsSingleton();
            services.Add<IDeviceFingerprintProvider>().ImplementedBy<DeviceFingerprintProvider>().AsSingleton();
            
            services.Add<IJsonSerializer>().ImplementedBy<JsonDotNetSerializer>().AsSingleton();
            
            services.InstallCoreComponents();
        }
    }

    class CoreLauncher : ModuleLauncher
    {
        [Inject]
        public IInitializablesRegistry InitializablesRegistry { get; set; }

        public override Priority Priority => Priority.Highest;

        public override void Launch()
        {
            InitializablesRegistry.StartInitializables();
        }
    }
}

public static class UnibricsCoreInstallerExtensions
{
    public static void InstallCoreComponents(this IServicesRegistry services)
    {
        services.Add<IExecutor>().ImplementedBy<Executor>().AsTransient();
        services.Add(typeof(IAttributedInstancesFactory<,>)).ImplementedBy(typeof(AttributedInstancesFactory<,>)).AsSingleton();
        services.Add(typeof(IInstalledInstancesFactory<>)).ImplementedBy(typeof(InstalledInstancesFactory<>)).AsSingleton();
        services.Add(typeof(ILazyGetter<>)).ImplementedBy(typeof(LazyInject<>)).AsTransient();
    }
}