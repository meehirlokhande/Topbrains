using System;

namespace Exceptions
{
    public class InvalidGPAException : Exception
    {
        public InvalidGPAException(string message) : base(message) { }
    }

    public class DuplicateStudentException : Exception
    {
        public DuplicateStudentException(string message) : base(message) { }
    }

    public class StudentNotFoundException : Exception
    {
        public StudentNotFoundException(string message) : base(message) { }
    }
}
