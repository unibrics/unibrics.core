namespace Unibrics.Core
{
    using DI;
    using Execution;

    interface IAppLauncher
    {
        void Launch();
    }

    public abstract class AppLauncher : IAppLauncher
    {
        [Inject]
        public IExecutor Executor { get; set; }

        public abstract void Launch();
    }
}