namespace Unibrics.Core.DI.Environment
{
    using System;
    using UnityEngine;

    public abstract class EnvironmentProvider : MonoBehaviour
    {
        public abstract IEnvironment GetEnvironment();
    }
    
    public abstract class EnvironmentProvider<T> : EnvironmentProvider where T : IEnvironment, new()
    {
        public override IEnvironment GetEnvironment() => new T();
    }
}