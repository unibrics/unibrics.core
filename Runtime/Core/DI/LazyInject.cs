namespace Unibrics.Core.DI
{
    public interface ILazyGetter<out T>
    {
        T Value { get; }
    }
    
    public class LazyInject<T> : ILazyGetter<T>
    {
        public T Value => instance ??= instanceProvider.Resolve<T>();

        private T instance;

        private readonly IResolver instanceProvider;

        public LazyInject(IResolver instanceProvider)
        {
            this.instanceProvider = instanceProvider;
        }
    }
}