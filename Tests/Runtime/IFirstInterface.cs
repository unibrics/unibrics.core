namespace Unibrics.Core.Tests
{
    interface IFirstInterface
    {
        
    }

    interface ISecondInterface
    {
        
    }

    interface IThirdInterface
    {
        
    }

    interface IFourthInterface
    {
        
    }
    
    class FirstImplementation : IFirstInterface, ISecondInterface, IThirdInterface, IFourthInterface
    {
    }

    class SecondImplementation : IFirstInterface, ISecondInterface, IThirdInterface, IFourthInterface
    {
        
    }
}