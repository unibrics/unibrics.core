namespace Unibrics.Core.Launchers
{
    public interface IModuleLauncher<TInstaller> where TInstaller : ModuleInstaller
    {
        void Launch();
    }
    
    
}