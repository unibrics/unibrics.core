namespace Unibrics.Core.Version
{
    using UnityEngine;

    public interface IVersionProvider
    {
        string FullVersion { get; }
    }

    class AppVersionProvider : IVersionProvider
    {
        public string FullVersion => Application.version;
    }
}