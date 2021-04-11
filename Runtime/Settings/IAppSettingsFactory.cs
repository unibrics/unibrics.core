namespace Unibrics.Core.Config
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Tools;
    using UnityEngine;
    using Types = Tools.Types;

    interface IAppSettingsFactory
    {
        IAppSettings LoadAppSettings();
    }

    class AppSettingsFactory : IAppSettingsFactory
    {
        public IAppSettings LoadAppSettings()
        {
            var raw = Resources.Load<TextAsset>("unibrics")?.text;
            var configTypes = Types.AnnotatedWith<InstallWithIdAttribute>().WithParent(typeof(IAppSettingsComponent))
                .ToDictionary(tuple => tuple.attribute.Id, tuple => tuple.type);
            return new AppSettings(GetComponents(raw, configTypes));
        }

        private List<IAppSettingsComponent> GetComponents(string raw, IDictionary<string, Type> settingsTypes)
        {
            var components = new List<IAppSettingsComponent>();
            if (raw == null)
            {
                return components;
            }
            
            var json = JObject.Parse(raw);
            foreach (var entry in json)
            {
                if (!settingsTypes.TryGetValue(entry.Key, out var type))
                {
                    continue;
                }
                
                components.Add((IAppSettingsComponent) entry.Value.ToObject(type));
            }

            return components;
        }
    }
}