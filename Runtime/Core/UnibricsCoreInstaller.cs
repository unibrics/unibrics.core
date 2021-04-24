namespace Unibrics.Core
{
    using System.Collections.Generic;
    using DI;
    using Execution;
    using Features;
    using Launchers;
    using Services;

    [Install]
    class UnibricsCoreInstaller : ModuleInstaller<CoreLauncher>
    {
        public override Priority Priority => Priority.Highest;

        public override void Install(IServicesRegistry services)
        {
            services.Add<IExecutor>().ImplementedBy<Executor>().AsSingleton();
            services.Add(typeof(IFeatureSet), typeof(IInitializable)).ImplementedBy<FeatureSet>().AsSingleton();
            services.Add<IFeatureSuspender>().ImplementedBy<FeatureSuspender>().AsSingleton();
        }
    }

    class CoreLauncher : IModuleLauncher
    {
        [Inject]
        public List<IInitializable> Initializables { get; set; }

        public void Launch()
        {
            foreach (var initializable in Initializables)
            {
                initializable.Initialize();
            }
        }
    }
}