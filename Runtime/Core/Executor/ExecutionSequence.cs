using UnityEngine;
using Logger = Unibrics.Logs.Logger;

namespace Unibrics.Core.Execution
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DI;

    public interface IExecutionSequence
    {
        event Action<IExecutableCommand> CommandStarted;
        
        IExecutionSequence AndThen<T>() where T : IExecutableCommand;
        IExecutionSequence AndThen(Action action);
        IExecutionSequence AndThen<T>(T command) where T : IExecutableCommand;
    }

    class ExecutionSequence : IExecutionSequence
    {
        public event Action<IExecutableCommand> CommandStarted;
        
        private readonly IInstanceProvider instanceProvider;
        
        private IExecutableCommand current;

        private readonly List<Func<IExecutableCommand>> queue = new();

        private readonly IInjector injector;

        public ExecutionSequence(IExecutableCommand current, IInstanceProvider instanceProvider, IInjector injector)
        {
            this.current = current;
            this.instanceProvider = instanceProvider;
            this.injector = injector;
            Start(current);
        }
        
        public IExecutionSequence AndThen<T>() where T : IExecutableCommand
        {
            queue.Add(() => instanceProvider.GetInstance<T>());
            TryPickNextCommand();
            return this;
        }

        public IExecutionSequence AndThen(Action action)
        {
            return AndThen(new LambdaExecutionCommand(action));
        }

        public IExecutionSequence AndThen<T>(T t) where T : IExecutableCommand
        {
            queue.Add(() =>
            {
                injector.InjectTo(t);
                return t;
            });
            TryPickNextCommand();
            return this;
        }

        private void Start(IExecutableCommand next)
        {
            Logger.Log("Execution", $"Starting executing {next}");
            CommandStarted?.Invoke(next);
            next.Execute(OnComplete);
        }

        private void OnComplete(ExecutionResult result)
        {
            switch (result)
            {
                case ExecutionResult.Complete:
                    current = null;
                    TryPickNextCommand();
                    break;
                case ExecutionResult.Error:
                case ExecutionResult.Aborted:
                    queue.Clear();
                    break;
            }
            
        }

        private void TryPickNextCommand()
        {
            if (!queue.Any() || current != null)
            {
                return;
            }
            
            var next = current = queue[0]();
            queue.RemoveAt(0);
            Start(next);
        }
    }
}