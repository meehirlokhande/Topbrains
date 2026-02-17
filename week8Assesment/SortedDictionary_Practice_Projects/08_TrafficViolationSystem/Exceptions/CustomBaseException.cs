using System;

namespace Exceptions
{
    public class InvalidFineAmountException : Exception
    {
        public InvalidFineAmountException(string message) : base(message) { }
    }

    public class DuplicateViolationException : Exception
    {
        public DuplicateViolationException(string message) : base(message) { }
    }
}
