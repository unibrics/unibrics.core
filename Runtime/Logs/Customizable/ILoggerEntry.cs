namespace Unibrics.Logs
{
    using UnityEngine;

    interface ILoggerEntry
    {
        bool IsEnabled { get; }
        
        Color Color { get; }
    }
}