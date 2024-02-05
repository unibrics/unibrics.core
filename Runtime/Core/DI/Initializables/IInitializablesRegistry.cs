namespace Unibrics.Core.DI
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    public interface IInitializablesRegistry
    {
        /// <summary>
        /// Start all IInitializable from application scope
        /// </summary>
        void StartInitializables();
        
        /// <summary>
        /// Start all new IInitializable from the provided scope
        /// </summary>
        void StartNewInitializables(IDependencyInjectionService diService);
    }

    class InitializablesRegistry : IInitializablesRegistry
    {
        [Inject]
        public List<IInitializable> Initializables { get; set; }

        private readonly ISet<IInitializable> started = new HashSet<IInitializable>();

        public void StartInitializables()
        {
            foreach (var initializable in Initializables.OrderByDescending(initializable => initializable.InitializationPriority))
            {
                initializable.Initialize();
                started.Add(initializable);
            }
        }

        public void StartNewInitializables(IDependencyInjectionService diService)
        {
            var allInitializables = diService.Resolver.Resolve<List<IInitializable>>()
                .OrderByDescending(initializable => initializable.InitializationPriority);

            foreach (var initializable in allInitializables)
            {
                if (started.Contains(initializable))
                {
                    continue;
                }
                
                initializable.Initialize();
                started.Add(initializable);
            }
            
        }
    }
}