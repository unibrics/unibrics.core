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
        private readonly List<AppFeature> features = new List<AppFeature>();

        private readonly IFeatureSuspender suspender;

        public FeatureSet(IFeatureSuspender suspender)
        {
            this.suspender = suspender;
        }

        public AppFeature GetFeatureById(string id)
        {
            return features.FirstOrDefault(feature => feature.Id == id);
        }

        public AppFeature GetFeature<T>() where T : AppFeature
        {
            return features.OfType<T>().FirstOrDefault();
        }

        public IEnumerable<AppFeature> AllFeatures => features;

        public void Initialize()
        {
            features.AddRange(Types.AnnotatedWith<AppFeatureAttribute>()
                .TypesOnly()
                .CreateInstances<AppFeature>());

            suspender.ProcessSuspendedFeatures(features);
        }
    }
}