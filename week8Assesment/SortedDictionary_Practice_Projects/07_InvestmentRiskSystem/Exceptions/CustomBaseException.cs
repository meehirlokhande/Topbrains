using System;

namespace Exceptions
{
    public class InvalidRiskRatingException : Exception
    {
        public InvalidRiskRatingException(string message) : base(message) { }
    }

    public class DuplicateInvestmentException : Exception
    {
        public DuplicateInvestmentException(string message) : base(message) { }
    }
}
