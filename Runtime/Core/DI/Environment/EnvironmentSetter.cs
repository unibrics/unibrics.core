namespace Unibrics.Core.DI.Environment
{
    using System;
    using UnityEngine;

    public abstract class EnvironmentSetter<T> : MonoBehaviour where T : IEnvironment, new()
    {
        [Inject]
        IEnvironmentSetter Setter { get; set; }
        
        public void Start()
        {
            Setter.SetCurrentEnvironmentTo<T>();
        }
    }
}