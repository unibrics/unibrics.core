namespace Unibrics.Core.Services
{
    public interface IFromBinding
    {
        IToBinding To<TTo>();

        IToBinding To<TTo>(TTo toObject);
    }

    public interface IFromBinding<TFrom>
    {
        IToBinding To<TTo>() where TTo : TFrom;

        IToBinding To<TTo>(TTo toObject) where TTo : TFrom;
    }

    public interface IToBinding
    {
        void AsSingleton();

        void AsTransient();
    }
}