using System;

namespace FlexibleInventorySystem_Practice.Exceptions
{
    /// <summary>
    /// Custom exception for inventory errors (e.g. duplicate ID, invalid data).
    /// </summary>
    public class InventoryException : Exception
    {
        public string ErrorCode { get; }

        public InventoryException() : base()
        {
            ErrorCode = "";
        }

        public InventoryException(string message) : base(message)
        {
            ErrorCode = "";
        }

        public InventoryException(string message, Exception inner) : base(message, inner)
        {
            ErrorCode = "";
        }

        public InventoryException(string message, string errorCode) : base(message)
        {
            ErrorCode = errorCode ?? "";
        }

        public override string Message
        {
            get
            {
                if (string.IsNullOrEmpty(ErrorCode))
                    return base.Message;
                return $"[{ErrorCode}] {base.Message}";
            }
        }
    }
}
