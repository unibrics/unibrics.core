namespace Unibrics.Core.Features
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DI;
    using Tools;

    public interface IFeatureSet
    {
        IEnumerable<AppFeature> AllFeatures { get; }
        AppFeature GetFeatureById(string id);

        AppFeature GetFeature<T>() where T : AppFeature;
    }

    internal class FeatureSet : IFeatureSet, IInitializable
    {
        public IEnumerable<AppFeature> AllFeatures => features;

        public Priority InitializationPriority => Priority.Highest;
        
        private readonly List<AppFeature> features = new List<AppFeature>();

        private readonly IFeatureSuspender suspender;

        public FeatureSet(IFeatureSuspender suspender)
        {
            this.suspender = suspender;
        }

        public AppFeature GetFeatureById(string id)
        {
            // in case some code will try to get features during initialization period
            if (!features.Any())
            {
                Initialize();
            }
            return features.FirstOrDefault(feature => feature.Id == id);
        }

        public AppFeature GetFeature<T>() where T : AppFeature
        {
            if (!features.Any())
            {
                Initialize();
            }
            return features.OfType<T>().FirstOrDefault();
        }
        
        public void Initialize()
        {
            if (features.Any())
            {
                return;
            }
            features.AddRange(Types.AnnotatedWith<AppFeatureAttribute>()
                .TypesOnly()
                .CreateInstances<AppFeature>());

            suspender.ProcessSuspendedFeatures(features);
        }
    }
}