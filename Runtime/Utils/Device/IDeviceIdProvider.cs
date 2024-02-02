namespace Unibrics.Core.Utils
{
    using System;
    using System.Security.Cryptography;
    using System.Text;

    public interface IDeviceIdProvider
    {
        string DeviceId { get; }
    }

}