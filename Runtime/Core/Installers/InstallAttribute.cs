namespace Unibrics.Core
{
    using System;
    using JetBrains.Annotations;

    [AttributeUsage(AttributeTargets.Class), MeansImplicitUse]
    public class InstallAttribute : Attribute
    {
        
    }
    
    [AttributeUsage(AttributeTargets.Class), MeansImplicitUse]
    public class InstallWithIdAttribute : Attribute
    {
        public string Id { get; }

        public InstallWithIdAttribute(string id)
        {
            Id = id;
        }
    }
}