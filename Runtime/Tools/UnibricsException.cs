namespace Unibrics.Tools
{
    using System;

    public class UnibricsException : Exception
    {
        public UnibricsException()
        {
        }

        public UnibricsException(string message) : base(message)
        {
        }

        public UnibricsException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}