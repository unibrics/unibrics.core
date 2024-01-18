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
            services.Add<IExecutor>().ImplementedBy<Executor>().AsTransient();
            services.Add<IEnvironmentChecker, IEnvironmentSetter>().ImplementedBy<EnvironmentHandler>().AsSingleton();
            services.Add<IFeatureSet, IInitializable>().ImplementedBy<FeatureSet>().AsSingleton();
            services.Add<IFeatureSuspender>().ImplementedBy<FeatureSuspender>().AsSingleton();
            services.Add<IVersionProvider>().ImplementedBy<AppVersionProvider>().AsSingleton();
            services.Add(typeof(IAttributedInstancesFactory<,>)).ImplementedBy(typeof(AttributedInstancesFactory<,>)).AsSingleton();
            services.Add(typeof(ILazyGetter<>)).ImplementedBy(typeof(LazyInject<>)).AsTransient();
            services.Add<IDeviceIdProvider>().ImplementedBy<DeviceIdProvider>().AsSingleton();
            services.Add<IDeviceFingerprintProvider>().ImplementedBy<IDeviceFingerprintProvider>().AsSingleton();
            
            services.Add<IJsonSerializer>().ImplementedBy<JsonDotNetSerializer>().AsSingleton();
        }
    }

    class CoreLauncher : ModuleLauncher
    {
        [Inject]
        public List<IInitializable> Initializables { get; set; }

        public override Priority Priority => Priority.Highest;

        public override void Launch()
        {
            foreach (var initializable in Initializables.OrderByDescending(i => i.InitializationPriority))
            {
                initializable.Initialize();
            }
        }
    }
}