namespace Unibrics.Core.Execution
{
    using System;

    public class LambdaExecutionCommand : ExecutableCommand
    {
        private readonly Action action;

        public LambdaExecutionCommand(Action action)
        {
            this.action = action;
        }

        protected override void ExecuteInternal()
        {
            action();
        }
    }
}