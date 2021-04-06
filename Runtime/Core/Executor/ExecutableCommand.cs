namespace Unibrics.Core.Execution
{
    using System;
   // using Logging;


    public interface IExecutableCommand
    {
        void Execute(Action<ExecutionResult> onComplete);
    }

    public abstract class ExecutableCommand : IExecutableCommand
    {
        private bool retained;

        private Action<ExecutionResult> onComplete;
        
        public void Execute(Action<ExecutionResult> onComplete)
        {
            try
            {
                ExecuteInternal();
            }
            catch(Exception e)
            {
                //Logger.Log($"EXCEPTION while executing {this}: {e.Message}\n{e.StackTrace}");
                throw e;
            }

            if (!retained)
            {
                onComplete.Invoke(ExecutionResult.Complete);
            }
            else
            {
                this.onComplete = onComplete;
            }
        }

        protected abstract void ExecuteInternal();

        protected void Retain()
        {
            retained = true;
        }

        protected void ReleaseAndComplete()
        {
            if (retained)
            {
                retained = false;
                onComplete?.Invoke(ExecutionResult.Complete);
            }
        }
    }

    public enum ExecutionResult
    {
        Complete,
        Error,
        Aborted
    }
}