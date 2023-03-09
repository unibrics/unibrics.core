namespace Unibrics.Logs
{
    using UnityEngine;

    public class CustomizableUnityLogger : ILogger
    {
        private readonly ILoggerSettings settings;
        
        public CustomizableUnityLogger()
        {
            settings = Resources.Load<LoggerSettings>("LoggerSettings");
            Debug.Log($"settings {settings} {Resources.Load<TextAsset>("unibrics")?.text}");
            if (settings == null)
            {
                settings = new DefaultLoggerSettings();
            }
        }

        public void Log(string message)
        {
            Debug.Log(message);
        }

        public void Log(string categoryKey, string message)
        {
            var entry = settings.GetEntryFor(categoryKey);
            if (!entry.IsEnabled)
            {
                return;
            }
            
            Debug.Log($"<color=#{ColorUtility.ToHtmlStringRGBA(entry.Color)}>{categoryKey} >>\t{message}</color>");
        }
    }
}