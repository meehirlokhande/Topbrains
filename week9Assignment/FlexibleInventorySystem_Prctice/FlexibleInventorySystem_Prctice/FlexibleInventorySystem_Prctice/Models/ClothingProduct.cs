using System;

namespace FlexibleInventorySystem_Practice.Models
{
    /// <summary>
    /// Clothing product with size, color and season.
    /// </summary>
    public class ClothingProduct : Product
    {
        public string Size { get; set; } = "";
        public string Color { get; set; } = "";
        public string Material { get; set; } = "";
        public string Gender { get; set; } = "Unisex";
        public string Season { get; set; } = "All-season";

        private static readonly string[] ValidSizes = { "XS", "S", "M", "L", "XL", "XXL" };

        public override string GetProductDetails()
        {
            return $"Size: {Size}, Color: {Color}, Material: {Material}";
        }

        public bool IsValidSize()
        {
            if (string.IsNullOrWhiteSpace(Size)) return false;
            foreach (var s in ValidSizes)
            {
                if (string.Equals(Size.Trim(), s, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }

        public override decimal CalculateValue()
        {
            decimal baseValue = Price * Quantity;
            string currentSeason = GetCurrentSeason();
            if (!string.Equals(Season, currentSeason, StringComparison.OrdinalIgnoreCase)
                && !string.Equals(Season, "All-season", StringComparison.OrdinalIgnoreCase))
                return baseValue * 0.85m;
            return baseValue;
        }

        private static string GetCurrentSeason()
        {
            int month = DateTime.Now.Month;
            if (month >= 3 && month <= 5) return "Summer";
            if (month >= 6 && month <= 8) return "Winter";
            if (month >= 9 && month <= 11) return "Summer";
            return "Winter";
        }
    }
}
