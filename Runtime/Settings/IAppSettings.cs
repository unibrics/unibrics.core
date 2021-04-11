namespace Unibrics.Core.Config
{
    public interface IAppSettings
    {
        T Get<T>() where T : IAppSettingsComponent;
    }

    public interface IAppSettingsComponent
    {
        
    }
}