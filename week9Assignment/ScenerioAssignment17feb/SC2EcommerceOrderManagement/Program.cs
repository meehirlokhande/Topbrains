#nullable disable
// Exceptions
class OutOfStockException : Exception { public OutOfStockException(string m) : base(m) { } }
class OrderAlreadyShippedException : Exception { public OrderAlreadyShippedException(string m) : base(m) { } }
class CustomerBlacklistedException : Exception { public CustomerBlacklistedException(string m) : base(m) { } }

class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public double Price { get; set; }
    public int Stock { get; set; }
}

class Customer
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool IsBlacklisted { get; set; }
}

class OrderItem
{
    public Product Product { get; set; }
    public int Quantity { get; set; }
    public double TotalPrice() => Product.Price * Quantity;
}

enum OrderStatus { Pending, Shipped, Delivered, Cancelled }

class Order
{
    public int OrderId { get; set; }
    public Customer Customer { get; set; }
    public List<OrderItem> Items { get; } = new List<OrderItem>();
    public DateTime OrderDate { get; set; }
    public OrderStatus OrderStatus { get; set; }
}

interface IDiscountStrategy
{
    double Apply(double amount);
}

class PercentageDiscount : IDiscountStrategy
{
    double _pct;
    public PercentageDiscount(double pct) { _pct = pct; }
    public double Apply(double amount) => amount * (1 - _pct / 100);
}

class FlatDiscount : IDiscountStrategy
{
    double _amount;
    public FlatDiscount(double amount) { _amount = amount; }
    public double Apply(double amount) => Math.Max(0, amount - _amount);
}

class FestivalDiscount : IDiscountStrategy
{
    public double Apply(double amount) => amount * 0.85; // 15% off
}

class OrderManager
{
    public List<Product> Products { get; } = new List<Product>();
    public List<Customer> Customers { get; } = new List<Customer>();
    public List<Order> Orders { get; } = new List<Order>();
    public Dictionary<int, Product> ProductDict { get; } = new Dictionary<int, Product>();
    IDiscountStrategy _discount;

    public void SetDiscount(IDiscountStrategy d) { _discount = d; }

    public Order PlaceOrder(int customerId, List<(int productId, int qty)> items)
    {
        var cust = Customers.First(c => c.Id == customerId);
        if (cust.IsBlacklisted) throw new CustomerBlacklistedException("Customer is blacklisted");
        var order = new Order { OrderId = Orders.Count + 1, Customer = cust, OrderDate = DateTime.Now, OrderStatus = OrderStatus.Pending };
        foreach (var (pid, qty) in items)
        {
            var p = ProductDict[pid];
            if (p.Stock < qty) throw new OutOfStockException($"{p.Name} has only {p.Stock} in stock");
            order.Items.Add(new OrderItem { Product = p, Quantity = qty });
            p.Stock -= qty;
        }
        Orders.Add(order);
        return order;
    }

    public void CancelOrder(int orderId)
    {
        var o = Orders.First(x => x.OrderId == orderId);
        if (o.OrderStatus == OrderStatus.Shipped || o.OrderStatus == OrderStatus.Delivered)
            throw new OrderAlreadyShippedException("Cannot cancel shipped order");
        o.OrderStatus = OrderStatus.Cancelled;
        foreach (var item in o.Items) item.Product.Stock += item.Quantity;
    }
}

class Program
{
    static OrderManager mgr = new OrderManager();

    static void Main(string[] args)
    {
        mgr.Products.Add(new Product { Id = 1, Name = "Laptop", Price = 50000, Stock = 20 });
        mgr.Products.Add(new Product { Id = 2, Name = "Mouse", Price = 500, Stock = 100 });
        mgr.Products.Add(new Product { Id = 3, Name = "Keyboard", Price = 2000, Stock = 5 });
        foreach (var p in mgr.Products) mgr.ProductDict[p.Id] = p;

        mgr.Customers.Add(new Customer { Id = 1, Name = "Amit" });
        mgr.Customers.Add(new Customer { Id = 2, Name = "Sneha", IsBlacklisted = false });

        var order1 = mgr.PlaceOrder(1, new List<(int, int)> { (1, 1), (2, 2) });
        order1.OrderDate = DateTime.Now.AddDays(-3);
        var order2 = mgr.PlaceOrder(2, new List<(int, int)> { (2, 5) });
        order2.OrderDate = DateTime.Now.AddDays(-10);
        mgr.Orders.Add(new Order { OrderId = 99, Customer = mgr.Customers[0], OrderDate = DateTime.Now.AddDays(-2), OrderStatus = OrderStatus.Shipped });
        mgr.Orders.Last().Items.Add(new OrderItem { Product = mgr.Products[0], Quantity = 1 });

        while (true)
        {
            Console.WriteLine("\n--- E-Commerce ---");
            Console.WriteLine("1. Place order  2. Cancel order  3. Set discount (Percent/Flat/Festival)");
            Console.WriteLine("4. Orders last 7 days  5. Total revenue  6. Most sold product  7. Top 5 customers  8. Group by status  9. Stock < 10  0. Exit");
            string c = Console.ReadLine();
            if (c == "0") break;
            try
            {
                if (c == "1") PlaceOrderMenu();
                else if (c == "2") { Console.Write("OrderId: "); mgr.CancelOrder(int.Parse(Console.ReadLine())); Console.WriteLine("Cancelled"); }
                else if (c == "3") SetDiscountMenu();
                else if (c == "4") LinqLast7Days();
                else if (c == "5") LinqRevenue();
                else if (c == "6") LinqMostSold();
                else if (c == "7") LinqTop5Customers();
                else if (c == "8") LinqGroupByStatus();
                else if (c == "9") LinqStockLess10();
            }
            catch (Exception ex) { Console.WriteLine("Error: " + ex.Message); }
        }
    }

    static void PlaceOrderMenu()
    {
        Console.Write("Customer id: ");
        int cid = int.Parse(Console.ReadLine());
        Console.Write("ProductId and Qty (e.g. 1 2): ");
        var parts = Console.ReadLine().Split();
        var items = new List<(int, int)> { (int.Parse(parts[0]), int.Parse(parts[1])) };
        var o = mgr.PlaceOrder(cid, items);
        Console.WriteLine("Order placed: " + o.OrderId);
    }

    static void SetDiscountMenu()
    {
        Console.Write("Type (1=Percent 2=Flat 3=Festival): ");
        string t = Console.ReadLine();
        if (t == "1") { Console.Write("Percent: "); mgr.SetDiscount(new PercentageDiscount(double.Parse(Console.ReadLine()))); }
        else if (t == "2") { Console.Write("Amount: "); mgr.SetDiscount(new FlatDiscount(double.Parse(Console.ReadLine()))); }
        else mgr.SetDiscount(new FestivalDiscount());
        Console.WriteLine("Discount set.");
    }

    static void LinqLast7Days()
    {
        var cutoff = DateTime.Now.AddDays(-7);
        var list = mgr.Orders.Where(o => o.OrderDate >= cutoff);
        foreach (var o in list) Console.WriteLine($"Order {o.OrderId} {o.OrderDate:d}");
    }

    static void LinqRevenue()
    {
        double rev = mgr.Orders.Where(o => o.OrderStatus != OrderStatus.Cancelled).SelectMany(o => o.Items).Sum(i => i.TotalPrice());
        Console.WriteLine("Total revenue: " + rev);
    }

    static void LinqMostSold()
    {
        var sold = mgr.Orders.Where(o => o.OrderStatus != OrderStatus.Cancelled).SelectMany(o => o.Items)
            .GroupBy(i => i.Product.Id).OrderByDescending(g => g.Sum(x => x.Quantity)).First();
        var p = mgr.ProductDict[sold.Key];
        Console.WriteLine("Most sold: " + p.Name);
    }

    static void LinqTop5Customers()
    {
        var top = mgr.Orders.Where(o => o.OrderStatus != OrderStatus.Cancelled)
            .GroupBy(o => o.Customer.Id).Select(g => new { Id = g.Key, Total = g.SelectMany(o => o.Items).Sum(i => i.TotalPrice()) })
            .OrderByDescending(x => x.Total).Take(5);
        foreach (var x in top) Console.WriteLine("Customer " + x.Id + " spent " + x.Total);
    }

    static void LinqGroupByStatus()
    {
        foreach (var g in mgr.Orders.GroupBy(o => o.OrderStatus))
        {
            Console.WriteLine(g.Key + ": " + g.Count());
        }
    }

    static void LinqStockLess10()
    {
        var list = mgr.Products.Where(p => p.Stock < 10);
        foreach (var p in list) Console.WriteLine(p.Name + " stock: " + p.Stock);
    }
}
