using System;

namespace Exceptions
{
    public class InvalidSeverityException : Exception
    {
        public InvalidSeverityException(string message) : base(message) { }
    }

    public class TicketNotFoundException : Exception
    {
        public TicketNotFoundException(string message) : base(message) { }
    }
}
