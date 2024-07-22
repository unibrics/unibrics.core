namespace Unibrics.Logs
{
    using System.Collections.Generic;

    public interface ILogAccessProvider
    {
        IEnumerable<string> GetLogs(string category);
    }
    
    class LogAccessProvider : ILogAccessProvider
    {
        public IEnumerable<string> GetLogs(string category)
        {
            return Logger.GetLogs(category);
        }
    }
}