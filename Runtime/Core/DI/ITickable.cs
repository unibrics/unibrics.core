namespace Unibrics.Core.DI
{
    public interface ITickable
    {
        /// <summary>
        /// Called every frame
        /// </summary>
        void Tick();
    }
}