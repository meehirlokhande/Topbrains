// Base product interface

/*

public interface IProduct
{
    int Id { get; }
    string Name { get; }
    decimal Price { get; set; }
    Category Category { get; }
}

public enum Category { Electronics, Clothing, Books, Groceries }

// 1. Create a generic repository for products
public class ProductRepository<T> where T : class, IProduct
{
    private List<T> _products = new List<T>();
    
    // TODO: Implement method to add product with validation
    public void AddProduct(T product)
    {
        // Rule: Product ID must be unique
        foreach (var p in _products)
        {
            if (p.Id == product.Id)
                throw new ArgumentException("Product ID already exists");
        }
        // Rule: Price must be positive
        if (product.Price <= 0)
            throw new ArgumentException("Price must be positive");
        // Rule: Name cannot be null or empty
        if (string.IsNullOrEmpty(product.Name))
            throw new ArgumentException("Name cannot be null or empty");
        // Add to collection if validation passes
        _products.Add(product);
    }
    
    // TODO: Create method to find products by predicate
    public IEnumerable<T> FindProducts(Func<T, bool> predicate)
    {
        // Should return filtered products
        var result = new List<T>();
        foreach (var p in _products)
        {
            if (predicate(p))
                result.Add(p);
        }
        return result;
    }
    
    // TODO: Calculate total inventory value
    public decimal CalculateTotalValue()
    {
        // Return sum of all product prices
        decimal total = 0;
        foreach (var p in _products)
        {
            total = total + p.Price;
        }
        return total;
    }
}

// 2. Specialized electronic product
public class ElectronicProduct : IProduct
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public decimal Price { get; set; }
    public Category Category => Category.Electronics;
    public int WarrantyMonths { get; set; }
    public string Brand { get; set; } = "";
}

// 3. Create a discounted product wrapper
public class DiscountedProduct<T> where T : IProduct
{
    private T _product;
    private decimal _discountPercentage;
    
    public DiscountedProduct(T product, decimal discountPercentage)
    {
        // TODO: Initialize with validation
        // Discount must be between 0 and 100
        if (discountPercentage < 0 || discountPercentage > 100)
            throw new ArgumentException("Discount must be between 0 and 100");
        _product = product;
        _discountPercentage = discountPercentage;
    }
    
    // TODO: Implement calculated price with discount
    public decimal DiscountedPrice => _product.Price * (1 - _discountPercentage / 100);
    
    // TODO: Override ToString to show discount details
    public override string ToString()
    {
        return $"{_product.Name} - Original: {_product.Price}, Discount: {_discountPercentage}%, Final: {DiscountedPrice}";
    }
}

// 4. Inventory manager with constraints
public class InventoryManager
{
    // TODO: Create method that accepts any IProduct collection
    public void ProcessProducts<T>(IEnumerable<T> products) where T : IProduct
    {
        // a) Print all product names and prices
        Console.WriteLine("--- All products ---");
        foreach (var p in products)
        {
            Console.WriteLine($"{p.Name} - ${p.Price}");
        }
        // b) Find the most expensive product
        IProduct? mostExpensive = null;
        foreach (var p in products)
        {
            if (mostExpensive == null || p.Price > mostExpensive.Price)
                mostExpensive = p;
        }
        if (mostExpensive != null)
            Console.WriteLine($"Most expensive: {mostExpensive.Name} - ${mostExpensive.Price}");
        // c) Group products by category
        var byCategory = new Dictionary<Category, List<IProduct>>();
        foreach (var p in products)
        {
            if (!byCategory.ContainsKey(p.Category))
                byCategory[p.Category] = new List<IProduct>();
            byCategory[p.Category].Add(p);
        }
        Console.WriteLine("--- By category ---");
        foreach (var kv in byCategory)
        {
            Console.Write($"{kv.Key}: ");
            foreach (var pr in kv.Value)
                Console.Write(pr.Name + " ");
            Console.WriteLine();
        }
        // d) Apply 10% discount to Electronics over $500
        Console.WriteLine("--- Electronics over $500 (10% off) ---");
        foreach (var p in products)
        {
            if (p.Category == Category.Electronics && p.Price > 500)
            {
                decimal discounted = p.Price * 0.9m;
                Console.WriteLine($"{p.Name}: ${p.Price} -> ${discounted}");
            }
        }
    }
    
    // TODO: Implement bulk price update with delegate
    public void UpdatePrices<T>(List<T> products, Func<T, decimal> priceAdjuster) 
        where T : IProduct
    {
        // Apply priceAdjuster to each product
        // Handle exceptions gracefully
        for (int i = 0; i < products.Count; i++)
        {
            try
            {
                decimal newPrice = priceAdjuster(products[i]);
                products[i].Price = newPrice;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating product {products[i].Name}: {ex.Message}");
            }
        }
    }
}

// 5. TEST SCENARIO: Your tasks:
// a) Implement all TODO methods with proper error handling
// b) Create a sample inventory with at least 5 products
// c) Demonstrate:
//    - Adding products with validation
//    - Finding products by brand (for electronics)
//    - Applying discounts
//    - Calculating total value before/after discount
//    - Handling a mixed collection of different product types

public class Program
{
    static void Main(string[] args)
    {
        var repo = new ProductRepository<ElectronicProduct>();
        var manager = new InventoryManager();

        // create at least 5 products
        var p1 = new ElectronicProduct { Id = 1, Name = "Laptop", Price = 999, Brand = "Dell", WarrantyMonths = 24 };
        var p2 = new ElectronicProduct { Id = 2, Name = "Phone", Price = 599, Brand = "Samsung", WarrantyMonths = 12 };
        var p3 = new ElectronicProduct { Id = 3, Name = "TV", Price = 650, Brand = "Sony", WarrantyMonths = 18 };
        var p4 = new ElectronicProduct { Id = 4, Name = "Headphones", Price = 150, Brand = "Dell", WarrantyMonths = 6 };
        var p5 = new ElectronicProduct { Id = 5, Name = "Monitor", Price = 300, Brand = "LG", WarrantyMonths = 12 };

        // adding products with validation
        repo.AddProduct(p1);
        repo.AddProduct(p2);
        repo.AddProduct(p3);
        repo.AddProduct(p4);
        repo.AddProduct(p5);

        Console.WriteLine("Total value before discount: " + repo.CalculateTotalValue());

        // finding products by brand (for electronics)
        var dellProducts = repo.FindProducts(p => p.Brand == "Dell");
        Console.WriteLine("Products by brand Dell:");
        foreach (var p in dellProducts)
            Console.WriteLine("  " + p.Name);

        // applying discounts
        var discounted1 = new DiscountedProduct<ElectronicProduct>(p1, 10);
        var discounted2 = new DiscountedProduct<ElectronicProduct>(p3, 15);
        Console.WriteLine(discounted1.ToString());
        Console.WriteLine(discounted2.ToString());

        // total value after discount (we just show the discounted price of those two, not full inventory)
        decimal totalAfter = 0;
        totalAfter += discounted1.DiscountedPrice + discounted2.DiscountedPrice;
        totalAfter += p2.Price + p4.Price + p5.Price;
        Console.WriteLine("Total value if we use discount on laptop and TV: " + totalAfter);

        // mixed collection of different product types - we use IProduct list
        var mixed = new List<IProduct>();
        mixed.Add(p1);
        mixed.Add(p2);
        mixed.Add(p3);
        manager.ProcessProducts(mixed);

        // bulk price update with delegate
        var listToUpdate = new List<ElectronicProduct> { p1, p2, p3 };
        manager.UpdatePrices(listToUpdate, (p) => p.Price * 1.1m);
        Console.WriteLine("After 10% increase - Laptop: " + p1.Price + ", Phone: " + p2.Price);
    }
}

*/

//------------------------------------------------------------------------------------------------------

// Base product interface
public interface IProduct
{
    int Id { get; }
    string Name { get; }
    decimal Price { get; }
    Category Category { get; }
}

public enum Category { Electronics, Clothing, Books, Groceries }

// 1. Create a generic repository for products
public class ProductRepository<T> where T : class, IProduct
{
    private List<T> _products = new List<T>();
    
    // TODO: Implement method to add product with validation
    public void AddProduct(T product)
    {
        // Rule: Product ID must be unique
        if(_products.Any(p=>p.Id == product.Id)){
            throw new ArgumentException("Product ID already exists");
        }else{
             // Rule: Price must be positive
            if(product.Price <0){
                throw new ArgumentException("Price must be positive");
            }
            // Rule: Name cannot be null or empty
            if(string.IsNullOrEmpty(product.Name)){
                throw new ArgumentException("Name cannot be null or empty");
            }
            // Add to collection if validation passes
            _products.Add(product);
        }  
    }
    
    // TODO: Create method to find products by predicate
    public IEnumerable<T> FindProducts(Func<T, bool> predicate)
    {
        // Should return filtered products
        

    }
    
    // TODO: Calculate total inventory value
    public decimal CalculateTotalValue()
    {
        // Return sum of all product prices
    }
}

// 2. Specialized electronic product
public class ElectronicProduct : IProduct
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public Category Category => Category.Electronics;
    public int WarrantyMonths { get; set; }
    public string Brand { get; set; }
}

// 3. Create a discounted product wrapper
public class DiscountedProduct<T> where T : IProduct
{
    private T _product;
    private decimal _discountPercentage;
    
    public DiscountedProduct(T product, decimal discountPercentage)
    {
        // TODO: Initialize with validation
        // Discount must be between 0 and 100
    }
    
    // TODO: Implement calculated price with discount
    public decimal DiscountedPrice => _product.Price * (1 - _discountPercentage / 100);
    
    // TODO: Override ToString to show discount details
}

// 4. Inventory manager with constraints
public class InventoryManager
{
    // TODO: Create method that accepts any IProduct collection
    public void ProcessProducts<T>(IEnumerable<T> products) where T : IProduct
    {
        // a) Print all product names and prices
        // b) Find the most expensive product
        // c) Group products by category
        // d) Apply 10% discount to Electronics over $500
    }
    
    // TODO: Implement bulk price update with delegate
    public void UpdatePrices<T>(List<T> products, Func<T, decimal> priceAdjuster) 
        where T : IProduct
    {
        // Apply priceAdjuster to each product
        // Handle exceptions gracefully
    }
}

// 5. TEST SCENARIO: Your tasks:
// a) Implement all TODO methods with proper error handling
// b) Create a sample inventory with at least 5 products
// c) Demonstrate:
//    - Adding products with validation
//    - Finding products by brand (for electronics)
//    - Applying discounts
//    - Calculating total value before/after discount
//    - Handling a mixed collection of different product types

