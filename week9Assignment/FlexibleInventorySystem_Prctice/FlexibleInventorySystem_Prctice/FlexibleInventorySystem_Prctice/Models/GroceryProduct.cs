using System;

namespace FlexibleInventorySystem_Practice.Models
{
    /// <summary>
    /// Grocery product with expiry and storage info.
    /// </summary>
    public class GroceryProduct : Product
    {
        public DateTime ExpiryDate { get; set; }
        public bool IsPerishable { get; set; }
        public double Weight { get; set; }
        public string StorageTemperature { get; set; } = "";

        public override string GetProductDetails()
        {
            return $"{Name}, Expires: {ExpiryDate:yyyy-MM-dd}, Storage: {StorageTemperature}";
        }

        public bool IsExpired()
        {
            return DateTime.Now.Date > ExpiryDate.Date;
        }

        public int DaysUntilExpiry()
        {
            var diff = (ExpiryDate.Date - DateTime.Now.Date).Days;
            return diff;
        }

        public override decimal CalculateValue()
        {
            decimal baseValue = Price * Quantity;
            if (DaysUntilExpiry() >= 0 && DaysUntilExpiry() <= 3)
                return baseValue * 0.80m;
            return baseValue;
        }
    }
}
