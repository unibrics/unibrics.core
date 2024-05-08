namespace Unibrics.Core
{
    using Threads;
    using UnityEngine;

    public interface IApplication
    {
        string PersistentDataPath { get; }

        string Version { get; }
        
        string DeviceId { get; }
    }
    
    class Application : IApplication, IMainThreadInitializable
    {
        public string PersistentDataPath { get; private set; }

        public string Version { get; private set; }

        public string DeviceId { get; private set; }
        
        public void Initialize()
        {
            PersistentDataPath = UnityEngine.Application.persistentDataPath;
            Version = UnityEngine.Application.version;
            DeviceId = SystemInfo.deviceUniqueIdentifier;
        }
    }
}