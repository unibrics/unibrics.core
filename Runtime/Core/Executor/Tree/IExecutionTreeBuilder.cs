namespace Unibrics.Core.Execution
{
    using DI;

    public interface IExecutionTreeBuilder
    {
        IExecutionTreeOptions CreateTree();
    }

    public interface IExecutionTreeOptions
    {
        IExecutionTree WithMainThreadAsDefault();
        
        IExecutionTree WithBackgroundThreadAsDefault();
    }

    public class ExecutionTreeBuilder : IExecutionTreeBuilder, IExecutionTreeOptions
    {
        private readonly IInstanceProvider instanceProvider;

        public ExecutionTreeBuilder(IInstanceProvider instanceProvider)
        {
            this.instanceProvider = instanceProvider;
        }

        public IExecutionTreeOptions CreateTree()
        {
            return this;
        }
        
        public IExecutionTree WithMainThreadAsDefault()
        {
            return new ExecutionTree(instanceProvider, true);
        }

        public IExecutionTree WithBackgroundThreadAsDefault()
        {
            return new ExecutionTree(instanceProvider, false);
        }
    }
}