namespace Unibrics.Core.Config
{
    using System.Collections.Generic;
    using System.Linq;

    class AppSettings : IAppSettings
    {
        private readonly IList<IAppSettingsSection> components;

        public AppSettings(IList<IAppSettingsSection> components)
        {
            this.components = components;
        }

        public T Get<T>() where T : IAppSettingsSection
        {
            return components.OfType<T>().FirstOrDefault();
        }

    }
}