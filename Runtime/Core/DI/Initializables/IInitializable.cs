namespace Unibrics.Core.DI
{
    public interface IInitializable
    {
        Priority InitializationPriority { get; }
        
        void Initialize();
    }
}