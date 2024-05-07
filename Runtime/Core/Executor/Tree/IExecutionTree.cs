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
        ICommandExecutionOptions AddCommand<T>() where T : IExecutableCommand;
        ICommandExecutionOptions AddCommand(IExecutableCommand command);

        ICommandExecutionOptions AddCommand(Action action);

        IExecutionTree AddSyncPoint();

        Task Execute();
    }

    class ExecutionTree : IExecutionTree
    {
        private readonly IInstanceProvider instanceProvider;

        private readonly List<List<CommandExecutionOptions>> bag = new() { new List<CommandExecutionOptions>() };

        private readonly ConcurrentQueue<Type> executedCommands = new();
        
        private readonly TaskCompletionSource<bool> finalTask = new();

        private TaskCompletionSource<CommandExecutionOptions> nextMainThreadCommandTcs;

        private int currentSection = 0;

        private bool mainThreadIsWaiting;

        public ExecutionTree(IInstanceProvider instanceProvider)
        {
            this.instanceProvider = instanceProvider;
        }

        public ICommandExecutionOptions AddCommand<T>() where T : IExecutableCommand
        {
            var options = new CommandExecutionOptions(() => instanceProvider.GetInstance<T>());
            bag[currentSection].Add(options);
            return options;
        }

        public ICommandExecutionOptions AddCommand(IExecutableCommand command)
        {
            var options = new CommandExecutionOptions(() => command);
            bag[currentSection].Add(options);
            return options;
        }

        public ICommandExecutionOptions AddCommand(Action action)
        {
            var options = new CommandExecutionOptions(() => new LambdaExecutionCommand(action));
            bag[currentSection].Add(options);
            return options;
        }

        public IExecutionTree AddSyncPoint()
        {
            if (bag[currentSection].Any())
            {
                bag.Add(new List<CommandExecutionOptions>());
                currentSection++;
            }
            return this;
        }

        private List<CommandExecutionOptions> GetAvailableBackgroundCommands()
        {
            var result = new List<CommandExecutionOptions>();

            lock (bag)
            {
                while (CycleThroughCurrentSection())
                {
                    currentSection++;
                }
            }

            return result;

            bool CycleThroughCurrentSection()
            {
                if (currentSection >= bag.Count)
                {
                    return false;
                }
                var isReadyToSwitchToNextSection = true;
                foreach (var options in bag[currentSection])
                {
                    if (options.Status != CommandExecutionStatus.Executed)
                    {
                        isReadyToSwitchToNextSection = false;
                    }
                    if (options.Status == CommandExecutionStatus.Waiting
                        && !options.IsMainThread
                        && options.DoesNotHaveUnfinishedPrerequisites(executedCommands))
                    {
                        options.OnCommandStarted();
                        result.Add(options);
                        isReadyToSwitchToNextSection = false;
                    }
                }

                return isReadyToSwitchToNextSection;
            }
        }

        private CommandExecutionOptions GetAvailableMainThreadCommand()
        {
            CommandExecutionOptions result = null;
            lock (bag)
            {
                while (CycleThroughCurrentSection(options => result = options))
                {
                    currentSection++;
                }
            }

            return result;

            bool CycleThroughCurrentSection(Action<CommandExecutionOptions> setter)
            {
                if (currentSection >= bag.Count)
                {
                    return false;
                }
                var isReadyToSwitchToNextSection = true;
                foreach (var options in bag[currentSection])
                {
                    Debug.Log($"checking for main thread candidate: {options}");
                    if (options.Status != CommandExecutionStatus.Executed)
                    {
                        isReadyToSwitchToNextSection = false;
                    }
                    if (options.Status == CommandExecutionStatus.Waiting
                        && options.IsMainThread
                        && options.DoesNotHaveUnfinishedPrerequisites(executedCommands))
                    {
                        setter(options);
                        isReadyToSwitchToNextSection = false;
                    }
                }

                return isReadyToSwitchToNextSection;
            }
        }

        /// <summary>
        /// Main entry point
        /// </summary>
        public async Task Execute()
        {
            // debug
            foreach (var list in bag)
            {
                Debug.Log($"list for execution: {list.Count}");
            }

            // preparation
            AddSyncPoint();
            bag[currentSection].Add(CommandExecutionOptions.FinalOptions(() => finalTask.TrySetResult(true)));

            // reset section
            currentSection = 0;

            // start
            var startingCommands = GetAvailableBackgroundCommands().ToList();
            for (var i = 0; i < startingCommands.Count; i++)
            {
                var closure = i;
                ThreadPool.QueueUserWorkItem(state =>
                {
                    Debug.Log($"Starting {closure}");
                    Start(startingCommands[closure], false, () =>
                    {
                        Debug.Log($"Completing {closure}");
                    });
                }, 38);
            }

            ExecuteMainThreadBranch();


            //WaitHandle.WaitAll(handles.ToArray());
            var res = await Task.WhenAny(Task.Delay(5000), finalTask.Task);

            Debug.Log($"Completing {finalTask.Task.IsCompleted}");
        }

        private async Task ExecuteMainThreadBranch()
        {
            Debug.Log($"starting main thread fetching...");
            var mainThreadCommand = GetAvailableMainThreadCommand();
            if (mainThreadCommand == null)
            {
                Debug.Log($"no command, will wait");
                foreach (var command in bag[currentSection])
                {
                    Debug.Log($"State: {command}");
                }
                nextMainThreadCommandTcs = new();
                mainThreadIsWaiting = true;
                mainThreadCommand = await nextMainThreadCommandTcs.Task.ConfigureAwait(true);
                mainThreadIsWaiting = false;
            }

            Debug.Log(
                $"fetched main thread command: {mainThreadCommand}, {Thread.CurrentThread.ManagedThreadId}, {mainThreadCommand.IsFinalCommand}");
            if (mainThreadCommand.IsFinalCommand)
            {
                Debug.Log($"final command reached, exit");
                mainThreadCommand.GetCommand().Execute(result => { });
                return;
            }

            mainThreadCommand.OnCommandStarted();
            Start(mainThreadCommand, true, async () =>
            {
                await ExecuteMainThreadBranch().ConfigureAwait(true);
            });
        }

        private void Start(CommandExecutionOptions nextOptions, bool isMainThread, Action onComplete)
        {
            //var nextOptions = GetAvailableCommands().FirstOrDefault();
            if (nextOptions == null)
            {
                Debug.Log($"command is null, skip");
                onComplete?.Invoke();
                return;
            }

            var next = nextOptions.GetCommand();
            Debug.Log($"command started (Thread#{Thread.CurrentThread.ManagedThreadId}): {next}");

            next.Execute(result =>
            {
                executedCommands.Enqueue(next.GetType());
                nextOptions.OnCommandExecuted();
                OnComplete(result, isMainThread);
                onComplete?.Invoke();
            });
        }

        private void TryStartAllPossibleCommands(bool isMainThread)
        {
            var commands = GetAvailableBackgroundCommands().ToList();
            Debug.Log($"Available commands: {commands.Count}");
            for (var index = 0; index < commands.Count; index++)
            {
                var command = commands[index];

                // priority to keep execution in current background thread
                // instead of scheduling another task to thread pool
                if (!isMainThread && index == 0)
                {
                    Start(command, false, () => { });
                }
                else
                {
                    ThreadPool.QueueUserWorkItem(_ =>
                    {
                        Start(command, false, () => { });
                    }, null);
                }
            }
        }

        private void OnComplete(ExecutionResult result, bool isMainThread)
        {
            switch (result)
            {
                case ExecutionResult.Complete:
                    Debug.Log($"Command complete");
                    TryStartAllPossibleCommands(isMainThread);
                    if (mainThreadIsWaiting)
                    {
                        lock (nextMainThreadCommandTcs)
                        {
                            var mainThreadCommand = GetAvailableMainThreadCommand();
                            if (mainThreadCommand != null)
                            {
                                if (nextMainThreadCommandTcs.TrySetResult(mainThreadCommand))
                                {
                                    mainThreadCommand.OnCommandStarted();
                                }
                            }
                        }
                    }
                    break;
                case ExecutionResult.Error:
                case ExecutionResult.Aborted:
                    Debug.LogError($"Execution aborted due to error");
                    bag.Clear();
                    finalTask.TrySetResult(false);
                    break;
            }
        }
    }
}