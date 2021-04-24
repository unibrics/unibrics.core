namespace Unibrics.Core.DI
{
    using System;

    public interface ITickProvider
    {
        ICancelable OnTick(Action action);
    }
}