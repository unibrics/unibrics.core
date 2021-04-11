namespace Unibrics.Core.Config
{
    using System.Collections.Generic;
    using System.Linq;

    class AppSettings : IAppSettings
    {
        private readonly IList<IAppSettingsComponent> components;

        public AppSettings(IList<IAppSettingsComponent> components)
        {
            this.components = components;
        }

        public T Get<T>() where T : IAppSettingsComponent
        {
            return components.OfType<T>().FirstOrDefault();
        }

    }
}