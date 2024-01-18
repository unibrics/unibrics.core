namespace Unibrics.Core.Utils
{
    /// <summary>
    /// If you need to mark something with device identity, but do not want to expose it's device ID explicitly
    /// </summary>
    public interface IDeviceFingerprintProvider
    {
        string DeviceFingerprint { get; }
    }
}