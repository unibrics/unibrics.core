namespace Unibrics.Core.Standalone
{
    using System.Collections.Generic;
    using System.Linq;
    using DI;
    using Execution;

    public class InstallStandaloneObjectsCommand : ExecutableCommand
    {
        [Inject]
        public IAttributedInstancesFactory<StandaloneAttribute, object> Factory { get; set; }

        private List<object> standaloneComponents;

        protected override void ExecuteInternal()
        {
            standaloneComponents = Factory.GetInstances().ToList();
        }
    }
}