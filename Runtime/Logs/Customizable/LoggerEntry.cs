namespace Unibrics.Logs
{
    using System;
    using UnityEngine;

    [Serializable]
    class LoggerEntry : ILoggerEntry
    {
        public string Category;

        public bool Enabled;

        public Color EntryColor;

        public bool IsEnabled => Enabled;

        public Color Color => EntryColor;
    }
}