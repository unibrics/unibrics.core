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
    
    internal class Executor : IExecutor
    {
        private readonly ILazyGetter<IInstanceProvider> instanceProvider;
        
        private readonly ILazyGetter<IInjector> injector;

        // we need lazy getters here, because DI environment is not constructed yet, so in order 
        // to use scene contexts in executable commands, injector must be resolved later
        public Executor(ILazyGetter<IInstanceProvider> instanceProvider, ILazyGetter<IInjector> injector)
        {
            this.injector = injector;
            this.instanceProvider = instanceProvider;
        }

        public IExecutionSequence Execute<T>() where T : IExecutableCommand
        {
            return new ExecutionSequence(instanceProvider.Value.GetInstance<T>(), instanceProvider.Value, injector.Value);
        }

        public IExecutionSequence Execute(IExecutableCommand command)
        {
            injector.Value.InjectTo(command);
            return new ExecutionSequence(command, instanceProvider.Value, injector.Value);
        }

        public IExecutionSequence Execute(Action action)
        {
            return Execute(new LambdaExecutionCommand(action));
        }
    }
}