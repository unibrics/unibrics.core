namespace Unibrics.Core.Utils
{
    using System;
    using System.Security.Cryptography;
    using System.Text;

    class DeviceFingerprintProvider : IDeviceFingerprintProvider
    {
        private string cachedFingerprint;

        private readonly IDeviceIdProvider deviceIdProvider;

        public DeviceFingerprintProvider(IDeviceIdProvider deviceIdProvider)
        {
            this.deviceIdProvider = deviceIdProvider;
        }
        
        public string DeviceFingerprint
        {
            get
            {
                if (cachedFingerprint != null)
                {
                    return cachedFingerprint;
                }

                cachedFingerprint =
                    Convert.ToBase64String(new SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes(deviceIdProvider.DeviceId)));
                return cachedFingerprint;
            }
        }
    }
}