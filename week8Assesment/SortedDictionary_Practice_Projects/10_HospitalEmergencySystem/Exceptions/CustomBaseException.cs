using System;

namespace Exceptions
{
    public class InvalidSeverityLevelException : Exception
    {
        public InvalidSeverityLevelException(string message) : base(message) { }
    }

    public class PatientNotFoundException : Exception
    {
        public PatientNotFoundException(string message) : base(message) { }
    }
}
