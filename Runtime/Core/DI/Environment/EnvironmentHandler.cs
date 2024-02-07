namespace Unibrics.Core.DI.Environment
{
    class EnvironmentHandler : IEnvironmentChecker
    {
        private readonly IEnvironment environment;

        public EnvironmentHandler(IEnvironment environment)
        {
            this.environment = environment;
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