namespace Unibrics.Core.Execution
{
    using DI;
    using Zenject;

    public interface IExecutor
    {
        IExecutionSequence Execute<T>() where T : IExecutableCommand;
        IExecutionSequence Execute(IExecutableCommand command);
    }
    
    internal class Executor : IExecutor
    {
        private readonly IInstanceProvider instanceProvider;
        
        private readonly IInjector injector;

        public Executor(IInstanceProvider instanceProvider, IInjector injector)
        {
            this.injector = injector;
            this.instanceProvider = instanceProvider;
        }

        public IExecutionSequence Execute<T>() where T : IExecutableCommand
        {
            return new ExecutionSequence(instanceProvider.GetInstance<T>(), instanceProvider, injector);
        }

        public IExecutionSequence Execute(IExecutableCommand command)
        {
            injector.InjectTo(command);
            return new ExecutionSequence(command, instanceProvider, injector);
        }
    }
}