namespace Unibrics.Logs
{
    using UnityEngine;

    public class SimpleUnityLogger : ILogger
    {
        public void Log(string message)
        {
            Debug.Log(message);
        }

        public void Log(string categoryKey, string message)
        {
            Debug.Log($"{categoryKey}>>>\t{message}");
        }
    }
}