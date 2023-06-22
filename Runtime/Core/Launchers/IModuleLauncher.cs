namespace Unibrics.Core.Launchers
{
    public interface IModuleLauncher
    {
        Priority Priority { get; } 
        
        void Launch();
    }

    public abstract class ModuleLauncher : IModuleLauncher
    {
        public virtual Priority Priority => Priority.Simple;

        public abstract void Launch();
    }
}