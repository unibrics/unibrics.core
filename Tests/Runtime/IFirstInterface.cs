namespace Unibrics.Core.Tests
{
    interface IFirstInterface
    {
        
    }

    interface ISecondInterface
    {
        
    }

    class FirstImplementation : IFirstInterface, ISecondInterface
    {
    }

    class SecondImplementation : IFirstInterface, ISecondInterface
    {
        
    }
}