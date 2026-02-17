using System;

namespace Exceptions
{
    public class InvalidFareException : Exception
    {
        public InvalidFareException(string message) : base(message) { }
    }

    public class DuplicateTicketException : Exception
    {
        public DuplicateTicketException(string message) : base(message) { }
    }

    public class TicketNotFoundException : Exception
    {
        public TicketNotFoundException(string message) : base(message) { }
    }
}
