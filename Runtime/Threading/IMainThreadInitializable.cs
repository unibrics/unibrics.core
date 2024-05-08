namespace Unibrics.Core.Threads
{
    /// <summary>
    /// Used for classes that need to be initialized from main thread before
    /// using in background ones
    /// </summary>
    public interface IMainThreadInitializable
    {
        void Initialize();
    }
}