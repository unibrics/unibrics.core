namespace Unibrics.Core.Services
{
    using System;

    public interface IFromBinding
    {
        IToTypeBinding ImplementedBy<TTo>();
        
        IToTypeBinding ImplementedBy(Type type);

        IToInstanceBinding ImplementedByInstance<TTo>(TTo toObject);
    }

    public interface IFromBinding<TFrom>
    {
        IToTypeBinding ImplementedBy<TTo>() where TTo : TFrom;
        
        IToTypeBinding ImplementedBy(Type type);

        IToInstanceBinding ImplementedByInstance<TTo>(TTo toObject) where TTo : TFrom;
    }

    public interface IToTypeBinding
    {
        void AsSingleton();

        void AsTransient();
    }

    public interface IToInstanceBinding
    {
        
    }
}