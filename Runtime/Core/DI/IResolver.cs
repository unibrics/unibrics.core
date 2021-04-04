namespace Unibrics.Core.DI
{
    using System;

    public interface IResolver
    {
        T Resolve<T>();

        object Resolve(Type type);
    }
}