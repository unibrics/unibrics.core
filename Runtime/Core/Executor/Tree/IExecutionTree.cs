namespace Unibrics.Core.Execution
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using DI;
    using UnityEngine;

    public interface IExecutionTree
    {
        IExecutionTree AddCommand<T>() where T : IExecutableCommand;

        Task Execute();
    }

    class ExecutionTree : IExecutionTree
    {
        private readonly IInstanceProvider instanceProvider;
        
        private readonly ConcurrentQueue<Func<IExecutableCommand>> queue = new();

        public ExecutionTree(IInstanceProvider instanceProvider)
        {
            this.instanceProvider = instanceProvider;
        }

        public IExecutionTree AddCommand<T>() where T : IExecutableCommand
        {
            queue.Enqueue(() => instanceProvider.GetInstance<T>());
            return this;
        }

        public async Task Execute()
        {
            var threads = 1;
            var handles = new List<WaitHandle>();
            for (int i = 0; i < 2; i++)
            {
                var closure = i;
                var tcs = new ManualResetEvent(false);
                handles.Add(tcs);
                //new Thread(o => StartNext(() => {})).Start();
                ThreadPool.QueueUserWorkItem(state =>
                {
                    Debug.Log($"Starting {closure}");
                    StartNext(() =>
                    {
                        Debug.Log($"Completing {closure}");
                        tcs.Set();
                    });
                });
                //StartNext();
            }

            WaitHandle.WaitAll(handles.ToArray());

        }

        private void StartNext(Action onComplete)
        {
            if (!queue.TryDequeue(out var func))
            {
                onComplete?.Invoke();
                return;
            }

            var next = func();
            next.Execute(result =>
            {
                OnComplete(result);
                onComplete?.Invoke();
            });
        }

        private void OnComplete(ExecutionResult result)
        {
            switch (result)
            {
                case ExecutionResult.Complete:
                    StartNext(() => {});
                    break;
                case ExecutionResult.Error:
                case ExecutionResult.Aborted:
                    queue.Clear();
                    break;
            }
        }
    }
}