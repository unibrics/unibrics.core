namespace Unibrics.Core.Execution
{
    using System;
    using DI;
    using UnityEngine;

    public interface IExecutor
    {
        IExecutionSequence Execute<T>() where T : IExecutableCommand;
        IExecutionSequence Execute(IExecutableCommand command);
        
        IExecutionSequence Execute(Action action);
    }
    
    public class Executor : IExecutor
    {
        private readonly IInstanceProvider instanceProvider;
        
        private readonly IInjector injector;

        // we need lazy getters here, because DI environment is not constructed yet, so in order 
        // to use scene contexts in executable commands, injector must be resolved later
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

        public IExecutionSequence Execute(Action action)
        {
            return Execute(new LambdaExecutionCommand(action));
        }
    }
}