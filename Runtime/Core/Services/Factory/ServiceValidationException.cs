namespace Unibrics.Core.Services
{
    using Tools;

    public class ServiceValidationException : UnibricsException
    {
        public ServiceValidationException(string message) : base(message)
        {
        }
    }
}