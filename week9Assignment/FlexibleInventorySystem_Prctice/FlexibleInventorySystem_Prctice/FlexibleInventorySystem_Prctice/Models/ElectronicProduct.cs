using System;

namespace FlexibleInventorySystem_Practice.Models
{
    /// <summary>
    /// Electronic product with brand, warranty and voltage.
    /// </summary>
    public class ElectronicProduct : Product
    {
        public string Brand { get; set; } = "";
        public int WarrantyMonths { get; set; }
        public string Voltage { get; set; } = "";
        public bool IsRefurbished { get; set; }

        public override string GetProductDetails()
        {
            return $"Brand: {Brand}, Model: {Name}, Warranty: {WarrantyMonths} months";
        }

        public DateTime GetWarrantyExpiryDate()
        {
            return DateAdded.AddMonths(WarrantyMonths);
        }

        public bool IsWarrantyValid()
        {
            return DateTime.Now < GetWarrantyExpiryDate();
        }
    }
}
