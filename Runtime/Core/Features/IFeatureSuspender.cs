namespace Unibrics.Core.Features
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using Unibrics.Utils;
    using Utils;

    public interface IFeatureSuspender
    {
        void ProcessSuspendedFeatures(List<AppFeature> features);

        void SetFeatureSuspendedTo(AppFeature feature, bool value);
    }

    internal class FeatureSuspender : IFeatureSuspender
    {
        private List<AppFeature> features;

        private readonly IDictionary<string, PlayerPrefsStoredValue<bool>> suspended =
            new Dictionary<string, PlayerPrefsStoredValue<bool>>();

        public void ProcessSuspendedFeatures(List<AppFeature> features)
        {
            this.features = features;
            SuspendNeededFeatures();
        }

        [Conditional("DEBUG")]
        private void SuspendNeededFeatures()
        {
            foreach (var feature in features)
            {
                if (feature.SupportsSuspension)
                {
                    PrepareFeature(feature);
                }
            }
        }

        private void PrepareFeature(AppFeature feature)
        {
            suspended[feature.Id] = new PlayerPrefsStoredValue<bool>(KeyFor(feature),
                feature.SetSuspendedTo, feature.DefaultSuspensionValue);
        }

        public void SetFeatureSuspendedTo(AppFeature feature, bool value)
        {
            TrySuspend(feature, value);
        }

        [Conditional("DEBUG")]
        private void TrySuspend(AppFeature feature, bool value)
        {
            if (!feature.SupportsSuspension)
            {
                return;
            }
            if (!suspended.ContainsKey(feature.Id))
            {
                PrepareFeature(feature);
            }

            suspended[feature.Id].Value = value;
        }

        private string KeyFor(AppFeature feature)
        {
            return $"Suspension.of.{feature.Id}";
        }
    }
}