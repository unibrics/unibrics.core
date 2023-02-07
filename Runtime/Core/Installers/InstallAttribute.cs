namespace Unibrics.Core
{
    using System;
    using JetBrains.Annotations;
    using UnityEngine.Scripting;

    [AttributeUsage(AttributeTargets.Class), MeansImplicitUse]
    public class InstallAttribute : PreserveAttribute
    {
        
    }
    
    [AttributeUsage(AttributeTargets.Class), MeansImplicitUse]
    public class InstallWithIdAttribute : PreserveAttribute
    {
        public string Id { get; }

        public InstallWithIdAttribute(string id)
        {
            Id = id;
        }
    }
}