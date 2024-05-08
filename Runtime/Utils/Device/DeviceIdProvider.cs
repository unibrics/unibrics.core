namespace Unibrics.Core.Utils
{
    using UnityEngine;

    class DeviceIdProvider : IDeviceIdProvider
    {
        private string cachedFingerprint;

        private string cachedId;

        public string DeviceId => cachedId ??= SystemInfo.deviceUniqueIdentifier;
    }
}