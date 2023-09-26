namespace Unibrics.Core.Processing
{
    interface IModuleInstallerProcessor
    {
        void Process(IModuleInstaller installer);
    }

    public abstract class TypedModulesInstallerProcessor<T> : IModuleInstallerProcessor where T : ModuleInstaller
    {
        public void Process(IModuleInstaller installer)
        {
            if (installer is T typed)
            {
                ProcessInternal(typed);
            }
        }

        protected abstract void ProcessInternal(T typedInstaller);
    }
}