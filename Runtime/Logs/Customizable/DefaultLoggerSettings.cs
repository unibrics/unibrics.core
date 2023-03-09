namespace Unibrics.Logs
{
    using UnityEngine;

    class DefaultLoggerSettings : ILoggerSettings
    {
        private readonly ILoggerEntry Entry = new LoggerEntry() { Enabled = true, EntryColor = Color.white };

        public ILoggerEntry GetEntryFor(string category) => Entry;
    }
}