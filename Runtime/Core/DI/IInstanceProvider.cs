namespace Unibrics.Core.DI
{
    using System;

    public interface IInstanceProvider
    {
        T GetInstance<T>();
        
        T GetInstance<T>(Type type);
        
        object GetInstance(Type type);
    }
}