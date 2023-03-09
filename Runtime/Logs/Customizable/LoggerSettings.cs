namespace Unibrics.Logs
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    [CreateAssetMenu(menuName = "Unibrics/Logger/Settings", fileName = "LoggerSettings")]
    public class LoggerSettings : ScriptableObject, ILoggerSettings
    {
        [SerializeField]
        private List<LoggerEntry> entries;

        private readonly LoggerEntry Disabled = new LoggerEntry() { Enabled = false, EntryColor = Color.white };

        public ILoggerEntry GetEntryFor(string category)
        {
            for (int i = 0; i < entries.Count; i++)
            {
                if (entries[i].Category == category)
                {
                    return entries[i];
                }
            }

            return Disabled;
        }
    }

    interface ILoggerSettings
    {
        ILoggerEntry GetEntryFor(string category);
    }

}