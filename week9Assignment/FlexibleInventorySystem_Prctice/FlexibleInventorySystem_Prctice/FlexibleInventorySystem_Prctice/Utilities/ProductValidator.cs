using FlexibleInventorySystem_Practice.Models;

namespace FlexibleInventorySystem_Practice.Utilities
{
    /// <summary>
    /// Validates product and derived product types before adding to inventory.
    /// </summary>
    public static class ProductValidator
    {
        public static bool ValidateProduct(Product product, out string errorMessage)
        {
            errorMessage = "";
            if (product == null)
            {
                errorMessage = "Product cannot be null.";
                return false;
            }
            if (string.IsNullOrWhiteSpace(product.Id))
            {
                errorMessage = "Product ID is required.";
                return false;
            }
            if (string.IsNullOrWhiteSpace(product.Name))
            {
                errorMessage = "Product name is required.";
                return false;
            }
            if (product.Price <= 0)
            {
                errorMessage = "Price must be greater than zero.";
                return false;
            }
            if (product.Quantity < 0)
            {
                errorMessage = "Quantity cannot be negative.";
                return false;
            }
            return true;
        }

        public static bool ValidateElectronicProduct(ElectronicProduct product, out string errorMessage)
        {
            errorMessage = "";
            if (product == null)
            {
                errorMessage = "Electronic product cannot be null.";
                return false;
            }
            if (!ValidateProduct(product, out errorMessage))
                return false;
            if (product.WarrantyMonths < 0)
            {
                errorMessage = "Warranty months cannot be negative.";
                return false;
            }
            return true;
        }

        public static bool ValidateGroceryProduct(GroceryProduct product, out string errorMessage)
        {
            errorMessage = "";
            if (product == null)
            {
                errorMessage = "Grocery product cannot be null.";
                return false;
            }
            if (!ValidateProduct(product, out errorMessage))
                return false;
            if (product.Weight < 0)
            {
                errorMessage = "Weight cannot be negative.";
                return false;
            }
            return true;
        }

        public static bool ValidateClothingProduct(ClothingProduct product, out string errorMessage)
        {
            errorMessage = "";
            if (product == null)
            {
                errorMessage = "Clothing product cannot be null.";
                return false;
            }
            if (!ValidateProduct(product, out errorMessage))
                return false;
            if (!product.IsValidSize())
            {
                errorMessage = "Invalid size. Use one of: XS, S, M, L, XL, XXL.";
                return false;
            }
            return true;
        }
    }
}
