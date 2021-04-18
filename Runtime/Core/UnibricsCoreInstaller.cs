namespace Unibrics.Core
{
    using Execution;
    using Services;

    [Install]
    class UnibricsCoreInstaller : ModuleInstaller
    {
        public override Priority Priority => Priority.Highest;
        
        public override void Install(IServicesRegistry services)
        {
            services.Add<IExecutor>().ImplementedBy<Executor>().AsSingleton();
        }
    }
}