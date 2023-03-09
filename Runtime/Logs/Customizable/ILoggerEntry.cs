namespace Unibrics.Logs
{
    using UnityEngine;

    public interface ILoggerEntry
    {
        bool IsEnabled { get; }
        
        Color Color { get; }
    }
}