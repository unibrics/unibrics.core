namespace Unibrics.Logs
{
    using System.Collections.Generic;
    using System.Diagnostics;

    public static class Logger
    {
        public static ILogger LoggerImplementation { get; set; } = new SimpleUnityLogger();

        private static readonly IDictionary<string, List<string>> history = new Dictionary<string, List<string>>();

        [Conditional("DEBUG")]
        public static void Log(string message)
        {
            LoggerImplementation?.Log(message);
        }

        internal static IEnumerable<string> GetLogs(string category)
        {
            return history.TryGetValue(category, out var list) ? list : new List<string>();
        }

        [Conditional("DEBUG")]
        public static void Log(string categoryKey, string message)
        {
            if (!history.TryGetValue(categoryKey, out var list))
            {
                list = new List<string>();
                history[categoryKey] = list;
            }
            list.Add(message);
            if (list.Count > 200)
            {
                list.RemoveAt(0);
            }
            LoggerImplementation?.Log(categoryKey, message);
        }
    }
}