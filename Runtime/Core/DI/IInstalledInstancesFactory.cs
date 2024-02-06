namespace Unibrics.Core.DI
{
    using System.Collections.Generic;

    public interface IInstalledInstancesFactory<TParent> 
    {
        IEnumerable<TParent> GetInstances();
    }

    class InstalledInstancesFactory<TParent> : AttributedInstancesFactory<InstallAttribute, TParent>, IInstalledInstancesFactory<TParent>
    {
        protected InstalledInstancesFactory(IInstanceProvider instanceProvider) : base(instanceProvider)
        {
        }
    }
}