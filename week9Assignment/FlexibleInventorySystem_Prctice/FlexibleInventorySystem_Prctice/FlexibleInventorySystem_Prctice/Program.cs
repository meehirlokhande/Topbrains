using System;
using FlexibleInventorySystem_Practice.Services;
using FlexibleInventorySystem_Practice.Models;
using FlexibleInventorySystem_Practice.Exceptions;
using FlexibleInventorySystem_Practice.Utilities;

namespace FlexibleInventorySystem_Practice
{
    class Program
    {
        private static InventoryManager _inventory = new InventoryManager();

        static void Main(string[] args)
        {
            while (true)
            {
                DisplayMenu();
                string choice = Console.ReadLine()?.Trim() ?? "";

                switch (choice)
                {
                    case "1":
                        AddProductMenu();
                        break;
                    case "2":
                        RemoveProductMenu();
                        break;
                    case "3":
                        UpdateQuantityMenu();
                        break;
                    case "4":
                        FindProductMenu();
                        break;
                    case "5":
                        ViewAllProducts();
                        break;
                    case "6":
                        ReportsMenu();
                        break;
                    case "7":
                        LowStockMenu();
                        break;
                    case "8":
                        Console.WriteLine("Goodbye.");
                        return;
                    default:
                        Console.WriteLine("Invalid option. Try again.");
                        break;
                }
                Console.WriteLine();
            }
        }

        static void DisplayMenu()
        {
            Console.WriteLine("----------------------------------------");
            Console.WriteLine("  FLEXIBLE INVENTORY SYSTEM");
            Console.WriteLine("----------------------------------------");
            Console.WriteLine("  1. Add Product");
            Console.WriteLine("  2. Remove Product");
            Console.WriteLine("  3. Update Quantity");
            Console.WriteLine("  4. Find Product");
            Console.WriteLine("  5. View All Products");
            Console.WriteLine("  6. Generate Reports");
            Console.WriteLine("  7. Check Low Stock");
            Console.WriteLine("  8. Exit");
            Console.WriteLine("----------------------------------------");
            Console.Write("Enter choice (1-8): ");
        }

        static void AddProductMenu()
        {
            Console.Write("Product type (1=Electronic, 2=Grocery, 3=Clothing): ");
            string type = Console.ReadLine()?.Trim() ?? "";
            Console.Write("Id: ");
            string id = Console.ReadLine()?.Trim() ?? "";
            Console.Write("Name: ");
            string name = Console.ReadLine()?.Trim() ?? "";
            Console.Write("Price: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal price) || price <= 0)
            {
                Console.WriteLine("Invalid price.");
                return;
            }
            Console.Write("Quantity: ");
            if (!int.TryParse(Console.ReadLine(), out int qty) || qty < 0)
            {
                Console.WriteLine("Invalid quantity.");
                return;
            }
            Console.Write("Category: ");
            string category = Console.ReadLine()?.Trim() ?? "";

            Product? product = null;
            if (type == "1")
            {
                Console.Write("Brand: ");
                string brand = Console.ReadLine()?.Trim() ?? "";
                Console.Write("Warranty (months): ");
                int.TryParse(Console.ReadLine(), out int warranty);
                Console.Write("Voltage: ");
                string voltage = Console.ReadLine()?.Trim() ?? "";
                product = new ElectronicProduct
                {
                    Id = id, Name = name, Price = price, Quantity = qty, Category = category,
                    Brand = brand, WarrantyMonths = warranty, Voltage = voltage
                };
            }
            else if (type == "2")
            {
                Console.Write("Expiry date (yyyy-mm-dd): ");
                if (!DateTime.TryParse(Console.ReadLine(), out DateTime expiry))
                    expiry = DateTime.Now.AddDays(7);
                Console.Write("Weight: ");
                double.TryParse(Console.ReadLine(), out double weight);
                Console.Write("Storage (e.g. Refrigerated): ");
                string storage = Console.ReadLine()?.Trim() ?? "";
                product = new GroceryProduct
                {
                    Id = id, Name = name, Price = price, Quantity = qty, Category = category,
                    ExpiryDate = expiry, Weight = weight, StorageTemperature = storage, IsPerishable = true
                };
            }
            else if (type == "3")
            {
                Console.Write("Size (XS/S/M/L/XL/XXL): ");
                string size = Console.ReadLine()?.Trim() ?? "";
                Console.Write("Color: ");
                string color = Console.ReadLine()?.Trim() ?? "";
                Console.Write("Material: ");
                string material = Console.ReadLine()?.Trim() ?? "";
                Console.Write("Gender (Men/Women/Unisex): ");
                string gender = Console.ReadLine()?.Trim() ?? "Unisex";
                Console.Write("Season (Summer/Winter/All-season): ");
                string season = Console.ReadLine()?.Trim() ?? "All-season";
                product = new ClothingProduct
                {
                    Id = id, Name = name, Price = price, Quantity = qty, Category = category,
                    Size = size, Color = color, Material = material, Gender = gender, Season = season
                };
            }
            else
            {
                Console.WriteLine("Unknown product type.");
                return;
            }

            if (product == null) return;
            try
            {
                if (_inventory.AddProduct(product))
                    Console.WriteLine("Product added.");
                else
                    Console.WriteLine("Failed to add: duplicate Id.");
            }
            catch (InventoryException ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        static void RemoveProductMenu()
        {
            Console.Write("Product Id to remove: ");
            string id = Console.ReadLine()?.Trim() ?? "";
            if (_inventory.RemoveProduct(id))
                Console.WriteLine("Product removed.");
            else
                Console.WriteLine("Product not found.");
        }

        static void UpdateQuantityMenu()
        {
            Console.Write("Product Id: ");
            string id = Console.ReadLine()?.Trim() ?? "";
            Console.Write("New quantity: ");
            if (!int.TryParse(Console.ReadLine(), out int qty) || qty < 0)
            {
                Console.WriteLine("Invalid quantity.");
                return;
            }
            if (_inventory.UpdateQuantity(id, qty))
                Console.WriteLine("Quantity updated.");
            else
                Console.WriteLine("Product not found.");
        }

        static void FindProductMenu()
        {
            Console.Write("Product Id: ");
            string id = Console.ReadLine()?.Trim() ?? "";
            var p = _inventory.FindProduct(id);
            if (p == null)
            {
                Console.WriteLine("Not found.");
                return;
            }
            Console.WriteLine(p.ToString());
            Console.WriteLine(p.GetProductDetails());
        }

        static void ViewAllProducts()
        {
            if (_inventory.GetTotalProductCount() == 0)
            {
                Console.WriteLine("No products in inventory.");
                return;
            }
            Console.WriteLine(_inventory.GenerateInventoryReport());
        }

        static void ReportsMenu()
        {
            Console.WriteLine("1. Full inventory  2. Category summary  3. Value report  4. Expiry report");
            Console.Write("Choice: ");
            string c = Console.ReadLine()?.Trim() ?? "";
            if (c == "1")
                Console.WriteLine(_inventory.GenerateInventoryReport());
            else if (c == "2")
                Console.WriteLine(_inventory.GenerateCategorySummary());
            else if (c == "3")
                Console.WriteLine(_inventory.GenerateValueReport());
            else if (c == "4")
            {
                Console.Write("Days until expiry: ");
                int.TryParse(Console.ReadLine(), out int days);
                Console.WriteLine(_inventory.GenerateExpiryReport(days <= 0 ? 7 : days));
            }
            else
                Console.WriteLine("Invalid choice.");
        }

        static void LowStockMenu()
        {
            Console.Write("Threshold (quantity below this is low stock): ");
            if (!int.TryParse(Console.ReadLine(), out int threshold))
                threshold = 10;
            var low = _inventory.GetLowStockProducts(threshold);
            if (low.Count == 0)
                Console.WriteLine("No low stock products.");
            else
            {
                Console.WriteLine($"Low stock (qty < {threshold}):");
                foreach (var p in low)
                    Console.WriteLine("  " + p.ToString());
            }
        }
    }
}
