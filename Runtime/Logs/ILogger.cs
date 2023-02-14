namespace Unibrics.Logs
{
    public interface ILogger
    {
        void Log(string message);

        void Log(string categoryKey, string message);
    }
}