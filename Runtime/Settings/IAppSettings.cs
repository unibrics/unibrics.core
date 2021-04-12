namespace Unibrics.Core.Config
{
    public interface IAppSettings
    {
        T Get<T>() where T : IAppSettingsSection;
    }

    public interface IAppSettingsSection
    {
        
    }
}