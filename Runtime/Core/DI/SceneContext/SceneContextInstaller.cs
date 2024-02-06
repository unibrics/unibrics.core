namespace Unibrics.Core.DI.SceneContext
{
    using Services;

    public abstract class SceneContextInstaller
    {
        public abstract string SceneName { get; }

        public virtual Priority Priority => Priority.Simple;
        
        public abstract void Install(IServicesRegistry services);
    }
}