using System;

namespace Exceptions
{
    public class InvalidFineException : Exception
    {
        public InvalidFineException(string message) : base(message) { }
    }

    public class MemberNotFoundException : Exception
    {
        public MemberNotFoundException(string message) : base(message) { }
    }
}
