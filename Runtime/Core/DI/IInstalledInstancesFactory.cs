namespace Unibrics.Core.DI
{
    using System.Collections.Generic;

    public interface IInstalledInstancesFactory<TParent> 
    {
        IEnumerable<TParent> GetInstances();
    }

    abstract class InstalledInstancesFactory<TParent> : AttributedInstancesFactory<InstallAttribute, TParent>
    {
        protected InstalledInstancesFactory(IInstanceProvider instanceProvider) : base(instanceProvider)
        {
        }
    }
}