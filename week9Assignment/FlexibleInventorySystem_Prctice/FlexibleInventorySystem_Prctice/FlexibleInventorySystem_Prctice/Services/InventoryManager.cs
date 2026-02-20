using System;
using System.Collections.Generic;
using System.Linq;
using FlexibleInventorySystem_Practice.Interfaces;
using FlexibleInventorySystem_Practice.Models;
using FlexibleInventorySystem_Practice.Utilities;
using FlexibleInventorySystem_Practice.Exceptions;

namespace FlexibleInventorySystem_Practice.Services
{
    /// <summary>
    /// Main inventory manager implementing inventory operations and reporting.
    /// </summary>
    public class InventoryManager : IInventoryOperations, IReportGenerator
    {
        private readonly List<Product> _products;
        private readonly object _lockObject = new object();

        public InventoryManager()
        {
            _products = new List<Product>();
        }

        public bool AddProduct(Product product)
        {
            if (product == null)
                throw new InventoryException("Product cannot be null.", "INV001");

            if (!ProductValidator.ValidateProduct(product, out string err))
                throw new InventoryException(err, "INV002");

            lock (_lockObject)
            {
                if (FindProduct(product.Id) != null)
                    return false;
                product.DateAdded = DateTime.Now;
                _products.Add(product);
            }
            return true;
        }

        public bool RemoveProduct(string productId)
        {
            lock (_lockObject)
            {
                var p = FindProduct(productId);
                if (p == null) return false;
                _products.Remove(p);
                return true;
            }
        }

        public Product FindProduct(string productId)
        {
            if (string.IsNullOrWhiteSpace(productId)) return null!;
            return _products.FirstOrDefault(p => string.Equals(p.Id, productId.Trim(), StringComparison.OrdinalIgnoreCase)) ?? null!;
        }

        public List<Product> GetProductsByCategory(string category)
        {
            if (string.IsNullOrWhiteSpace(category)) return new List<Product>();
            return _products.Where(p => string.Equals(p.Category, category.Trim(), StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public bool UpdateQuantity(string productId, int newQuantity)
        {
            if (newQuantity < 0) return false;
            var p = FindProduct(productId);
            if (p == null) return false;
            p.Quantity = newQuantity;
            return true;
        }

        public decimal GetTotalInventoryValue()
        {
            return _products.Sum(p => p.CalculateValue());
        }

        public List<Product> GetLowStockProducts(int threshold)
        {
            return _products.Where(p => p.Quantity < threshold).ToList();
        }

        public string GenerateInventoryReport()
        {
            var sb = new System.Text.StringBuilder();
            sb.AppendLine("=================================");
            sb.AppendLine("INVENTORY REPORT");
            sb.AppendLine("=================================");
            sb.AppendLine($"Total Products: {_products.Count}");
            sb.AppendLine($"Total Value: {GetTotalInventoryValue():C}");
            sb.AppendLine();
            sb.AppendLine("Product List:");
            foreach (var p in _products)
                sb.AppendLine($"  {p.Id} - {p.Name} - {p.Category} - {p.Quantity} - {p.CalculateValue():C}");
            return sb.ToString();
        }

        public string GenerateCategorySummary()
        {
            var sb = new System.Text.StringBuilder();
            sb.AppendLine("CATEGORY SUMMARY");
            var groups = _products.GroupBy(p => p.Category ?? "");
            foreach (var g in groups.OrderBy(x => x.Key))
            {
                decimal value = g.Sum(p => p.CalculateValue());
                sb.AppendLine($"{g.Key}: {g.Count()} items - Total Value: {value:C}");
            }
            return sb.ToString();
        }

        public string GenerateValueReport()
        {
            if (_products.Count == 0)
                return "No products in inventory.";

            var ordered = _products.OrderBy(p => p.CalculateValue()).ToList();
            var most = ordered.Last();
            var least = ordered.First();
            decimal avg = _products.Average(p => p.Price);
            var prices = _products.Select(p => p.Price).OrderBy(x => x).ToList();
            decimal median = prices.Count % 2 == 1
                ? prices[prices.Count / 2]
                : (prices[prices.Count / 2 - 1] + prices[prices.Count / 2]) / 2;
            int aboveAvg = _products.Count(p => p.Price > avg);

            var sb = new System.Text.StringBuilder();
            sb.AppendLine("VALUE REPORT");
            sb.AppendLine($"Most valuable: {most.Id} - {most.Name} - {most.CalculateValue():C}");
            sb.AppendLine($"Least valuable: {least.Id} - {least.Name} - {least.CalculateValue():C}");
            sb.AppendLine($"Average price: {avg:C}");
            sb.AppendLine($"Median price: {median:C}");
            sb.AppendLine($"Products above average price: {aboveAvg}");
            return sb.ToString();
        }

        public string GenerateExpiryReport(int daysThreshold)
        {
            var grocery = _products.OfType<GroceryProduct>().ToList();
            var expiring = grocery.Where(g => g.DaysUntilExpiry() >= 0 && g.DaysUntilExpiry() <= daysThreshold).ToList();
            var sb = new System.Text.StringBuilder();
            sb.AppendLine($"EXPIRY REPORT (within {daysThreshold} days)");
            if (expiring.Count == 0)
                sb.AppendLine("No products expiring in this period.");
            else
                foreach (var g in expiring)
                    sb.AppendLine($"  {g.Id} - {g.Name} - Expires in {g.DaysUntilExpiry()} days");
            return sb.ToString();
        }

        public IEnumerable<Product> SearchProducts(Func<Product, bool> predicate)
        {
            return _products.Where(predicate);
        }

        public void ApplyCategoryDiscount(string category, decimal discountPercentage)
        {
            if (discountPercentage < 0 || discountPercentage > 100) return;
            decimal factor = 1 - (discountPercentage / 100m);
            foreach (var p in GetProductsByCategory(category))
                p.Price = p.Price * factor;
        }

        public int GetTotalProductCount()
        {
            return _products.Count;
        }

        public IEnumerable<string> GetCategories()
        {
            return _products.Select(p => p.Category ?? "").Distinct();
        }
    }
}
