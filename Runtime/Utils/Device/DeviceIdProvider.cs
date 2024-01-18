namespace Unibrics.Core.Utils
{
    using UnityEngine;

    class DeviceIdProvider : IDeviceIdProvider
    {
        private string cachedFingerprint;
        
        public string DeviceId => SystemInfo.deviceUniqueIdentifier;
    }
}