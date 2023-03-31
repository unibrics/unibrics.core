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
        IExecutionSequence AndThen<T>() where T : IExecutableCommand;
        IExecutionSequence AndThen(Action action);
        IExecutionSequence AndThen<T>(T command) where T : IExecutableCommand;
    }

    class ExecutionSequence : IExecutionSequence
    {
        private readonly ILazyGetter<IInstanceProvider> instanceProvider;
        
        private IExecutableCommand current;

        private readonly List<Func<IExecutableCommand>> queue = new();

        private readonly ILazyGetter<IInjector> injector;

        public ExecutionSequence(IExecutableCommand current, ILazyGetter<IInstanceProvider> instanceProvider, ILazyGetter<IInjector> injector)
        {
            this.current = current;
            this.instanceProvider = instanceProvider;
            this.injector = injector;
            Start(current);
        }

        public IExecutionSequence AndThen<T>() where T : IExecutableCommand
        {
            queue.Add(() => instanceProvider.Value.GetInstance<T>());
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
                injector.Value.InjectTo(t);
                return t;
            });
            TryPickNextCommand();
            return this;
        }

        private void Start(IExecutableCommand next)
        {
            Logger.Log("Execution", $"Starting executing {next} at {Time.time}");
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
            if (queue.Any() && current == null)
            {
                current = queue[0]();
                queue.RemoveAt(0);
                Start(current);
            }
        }
    }
}