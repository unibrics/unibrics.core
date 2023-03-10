namespace Unibrics.Core.Features
{
    using Tools;

    public abstract class AppFeature
    {
        public virtual bool SupportsSuspension { get; }

        public virtual bool DefaultSuspensionValue => false;

        public virtual bool DoNotResetValueWithSave => false;

        protected virtual bool IsActiveByDefault => true;

        public bool IsSuspended
        {
            get
            {
                if (isSuspended.HasValue)
                {
                    return isSuspended.Value;
                }

                return !IsActiveByDefault;
            }
        }

        public abstract string Id { get; }

        private bool? isSuspended;

        public virtual void SetSuspendedTo(bool value)
        {
            if (!SupportsSuspension)
            {
                throw new UnibricsException($"Feature {GetType().Name} doesn't support suspension");
            }
            isSuspended = value;
        }
    }
}