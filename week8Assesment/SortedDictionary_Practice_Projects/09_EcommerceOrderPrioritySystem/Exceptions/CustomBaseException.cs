using System;

namespace Exceptions
{
    public class InvalidOrderAmountException : Exception
    {
        public InvalidOrderAmountException(string message) : base(message) { }
    }

    public class OrderNotFoundException : Exception
    {
        public OrderNotFoundException(string message) : base(message) { }
    }
}
