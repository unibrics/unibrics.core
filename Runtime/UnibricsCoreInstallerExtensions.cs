namespace Unibrics.Core
{
    using DI;
    using Execution;
    using Services;
    using Threads;

    public static class UnibricsCoreInstallerExtensions
    {
        public static void InstallCoreComponents(this IServicesRegistry services)
        {
            services.Add<IExecutor>().ImplementedBy<Executor>().AsTransient();
            services.Add<IApplication, IMainThreadInitializable>().ImplementedBy<Application>().AsSingleton();
            services.Add<IExecutionTreeBuilder>().ImplementedBy<ExecutionTreeBuilder>().AsTransient();
            services.Add(typeof(IAttributedInstancesFactory<,>)).ImplementedBy(typeof(AttributedInstancesFactory<,>))
                .AsSingleton();
            services.Add(typeof(IInstalledInstancesFactory<>)).ImplementedBy(typeof(InstalledInstancesFactory<>))
                .AsSingleton();
            services.Add(typeof(ILazyGetter<>)).ImplementedBy(typeof(LazyInject<>)).AsTransient();
        }
    }
}
