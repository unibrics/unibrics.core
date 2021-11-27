namespace Unibrics.Core.DI
{
    using System;

    public class ActivatorInstanceProvider : IInstanceProvider
    {
        public T GetInstance<T>()
        {
            return Activator.CreateInstance<T>();
        }

        public T GetInstance<T>(Type type)
        {
            return (T)Activator.CreateInstance(type);
        }

        public object GetInstance(Type type)
        {
            return Activator.CreateInstance(type);
        }
    }
}