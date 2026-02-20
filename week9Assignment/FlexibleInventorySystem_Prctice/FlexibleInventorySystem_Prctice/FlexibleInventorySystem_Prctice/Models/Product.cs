using System;

namespace FlexibleInventorySystem_Practice.Models
{
    /// <summary>
    /// Abstract base class for all products.
    /// </summary>
    public abstract class Product
    {
        public string Id { get; set; } = "";
        public string Name { get; set; } = "";
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string Category { get; set; } = "";
        public DateTime DateAdded { get; set; }

        /// <summary>
        /// Returns product-specific details (implemented by derived classes).
        /// </summary>
        public abstract string GetProductDetails();

        /// <summary>
        /// Calculates inventory value. Default is Price * Quantity.
        /// </summary>
        public virtual decimal CalculateValue()
        {
            return Price * Quantity;
        }

        /// <summary>
        /// Returns a short summary of the product.
        /// </summary>
        public override string ToString()
        {
            return $"{Id} - {Name} - {Price:C} - Qty: {Quantity}";
        }
    }
}
