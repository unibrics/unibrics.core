namespace Unibrics.Core.DI
{
    using System;
    using System.Collections.Generic;
    using Tools;

    public interface IAttributedInstancesFactory<TAttribute, TParent> where TAttribute : Attribute
    {
        IEnumerable<TParent> GetInstances();
    }

    class AttributedInstancesFactory<TAttribute, TParent> : IAttributedInstancesFactory<TAttribute, TParent> where TAttribute : Attribute
    {
        private readonly IInstanceProvider instanceProvider;

        public AttributedInstancesFactory(IInstanceProvider instanceProvider)
        {
            this.instanceProvider = instanceProvider;
        }

        public IEnumerable<TParent> GetInstances()
        {
            var types = Types.AnnotatedWith<TAttribute>().WithParent(typeof(TParent)).TypesOnly();

            foreach (var type in types)
            {
                yield return instanceProvider.GetInstance<TParent>(type);
            }
        }
    }
}