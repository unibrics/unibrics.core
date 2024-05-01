namespace Unibrics.Core.Execution
{
    using DI;

    public interface IExecutionTreeBuilder
    {
        IExecutionTree CreateTree();
    }

    public class ExecutionTreeBuilder : IExecutionTreeBuilder
    {
        private readonly IInstanceProvider instanceProvider;

        public ExecutionTreeBuilder(IInstanceProvider instanceProvider)
        {
            this.instanceProvider = instanceProvider;
        }

        public IExecutionTree CreateTree()
        {
            return new ExecutionTree(instanceProvider);
        }
    }
}