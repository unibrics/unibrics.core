namespace Unibrics.Core.Threads
{
    using System;
    using System.Collections.Generic;
    using Execution;

    public class InitializeMainThreadComponentsCommand : ExecutableCommand
    {
        private readonly List<IMainThreadInitializable> initializables;

        public InitializeMainThreadComponentsCommand(List<IMainThreadInitializable> initializables)
        {
            this.initializables = initializables;
        }

        protected override void ExecuteInternal()
        {
            if (!Threading.IsMainThread())
            {
                throw new Exception("This command should be called from the main thread");
            }

            foreach (var initializable in initializables)
            {
                initializable.Initialize();
            }
        }
    }
}