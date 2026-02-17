using System;

namespace Exceptions
{
    public class DuplicateMedicineException : Exception
    {
        public DuplicateMedicineException(string message) : base(message) { }
    }

    public class InvalidPriceException : Exception
    {
        public InvalidPriceException(string message) : base(message) { }
    }

    public class InvalidExpiryYearException: Exception
    {
        public InvalidExpiryYearException(string message):base(message) { }
    }
    public class MedicineNotFoundException: Exception
    {
        public MedicineNotFoundException(string message) : base(message){ }
    }
}
