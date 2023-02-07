namespace Unibrics.Core.DI
{
    using System;
    using UnityEngine.Scripting;

    [AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
    public class InjectAttribute : PreserveAttribute
    {
        
    }
}