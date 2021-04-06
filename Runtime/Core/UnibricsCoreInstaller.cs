namespace Unibrics.Core
{
    using Execution;
    using Services;

    [Install]
    public class UnibricsCoreInstaller : ModuleInstaller
    {
        public override Priority Priority => Priority.High;
        
        public override void Install(IServicesRegistry services)
        {
            services.AddSingleton<IExecutor, Executor>();
        }
    }
}