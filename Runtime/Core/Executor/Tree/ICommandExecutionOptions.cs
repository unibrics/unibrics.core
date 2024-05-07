namespace Unibrics.Core.Execution
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    public interface ICommandExecutionOptions
    {
        ICommandExecutionOptions After<T>() where T : IExecutableCommand;

        void InTheMainThread();
    }

    class CommandExecutionOptions : ICommandExecutionOptions
    {
        private readonly Func<IExecutableCommand> commandGetter;

        private List<Type> prerequisites;

        public CommandExecutionStatus Status { get; private set; } = CommandExecutionStatus.Waiting;

        public IExecutableCommand GetCommand()
        {
            command = commandGetter.Invoke();
            return command;
        }

        public bool IsMainThread { get; private set; }
        
        public bool IsFinalCommand { get; }

        private IExecutableCommand command;

        public CommandExecutionOptions(Func<IExecutableCommand> commandGetter)
        {
            this.commandGetter = commandGetter;
        }

        private CommandExecutionOptions(Action action)
        {
            commandGetter = () => new LambdaExecutionCommand(action);
            IsFinalCommand = true;
            IsMainThread = true;
        }

        internal static CommandExecutionOptions FinalOptions(Action action) => new(action);

        public void OnCommandExecuted()
        {
            Debug.Log($"Command {command} Executed!");
            Status = CommandExecutionStatus.Executed;
        }

        public void OnCommandStarted()
        {
            Debug.Log($"Command {command} Started!");
            Status = CommandExecutionStatus.Started;
        }

        public ICommandExecutionOptions After<T>() where T : IExecutableCommand
        {
            if (prerequisites == null)
            {
                prerequisites = new List<Type>();
            }
            prerequisites.Add(typeof(T));
            return this;
        }

        public void InTheMainThread()
        {
            IsMainThread = true;
        }

        public bool DoesNotHaveUnfinishedPrerequisites(ConcurrentQueue<Type> finished)
        {
            if (prerequisites == null)
            {
                return true;
            }

            foreach (var prerequisite in prerequisites)
            {
                if (!finished.Contains(prerequisite))
                {
                    return false;
                }
            }

            return true;
        }

        public override string ToString()
        {
            return
                $"{nameof(Status)}: {Status}, {nameof(IsMainThread)}: {IsMainThread}, {nameof(IsFinalCommand)}: {IsFinalCommand}";
        }
    }

    enum CommandExecutionStatus
    {
        Waiting, Started, Executed
    }
}