namespace Unibrics.Core.DI.Environment
{
    public interface IEnvironmentChecker
    {
        bool IsCurrentEnvironment<T>() where T : IEnvironment;
    }
}