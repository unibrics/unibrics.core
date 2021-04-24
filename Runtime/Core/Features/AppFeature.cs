namespace Unibrics.Core.Features
{
    using Tools;

    public abstract class AppFeature
    {
        public virtual bool SupportsSuspension { get; }

        public virtual bool DefaultSuspensionValue => false;

        public virtual bool DoNotResetValueWithSave => false;
        
        public bool IsSuspended { get; private set; }
        
        public abstract string Id { get; }

        public virtual void SetSuspendedTo(bool value)
        {
            if (!SupportsSuspension)
            {
                throw new UnibricsException($"Feature {GetType().Name} doesn't support suspension");
            }
            IsSuspended = value;
        }
    }
}