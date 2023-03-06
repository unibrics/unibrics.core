namespace Unibrics.Core.DI.Environment
{
    class EnvironmentHandler : IEnvironmentSetter, IEnvironmentChecker
    {
        private IEnvironment environment = new DefaultEnvironment();
        
        public void SetCurrentEnvironmentTo<T>() where T : IEnvironment, new()
        {
            environment = new T();
        }

        public bool IsCurrentEnvironment<T>() where T : IEnvironment
        {
            return environment is T;
        }
    }

    public class DefaultEnvironment : IEnvironment
    {
        
    }
}