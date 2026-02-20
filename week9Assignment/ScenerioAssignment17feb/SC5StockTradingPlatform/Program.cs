#nullable disable
class InvalidTradeException : Exception { public InvalidTradeException(string m) : base(m) { } }

class Stock
{
    public string Symbol { get; set; }
    public string Name { get; set; }
    public double Price { get; set; }
    public event Action<Stock, double> PriceChanged;

    public void UpdatePrice(double newPrice)
    {
        double old = Price;
        Price = newPrice;
        PriceChanged?.Invoke(this, old);
    }
}

class Investor
{
    public int Id { get; set; }
    public string Name { get; set; }
}

class Transaction
{
    public int Id { get; set; }
    public int InvestorId { get; set; }
    public string StockSymbol { get; set; }
    public int Quantity { get; set; }
    public double PricePerShare { get; set; }
    public DateTime Date { get; set; }
    public bool IsBuy { get; set; }
}

class Portfolio
{
    public int InvestorId { get; set; }
    public Dictionary<string, int> Holdings { get; } = new Dictionary<string, int>();
}

interface IRiskStrategy
{
    double CalculateRisk(Portfolio p, Dictionary<string, Stock> stocks);
}

class SimpleRiskStrategy : IRiskStrategy
{
    public double CalculateRisk(Portfolio p, Dictionary<string, Stock> stocks)
    {
        int totalShares = p.Holdings.Values.Sum();
        if (totalShares == 0) return 0;
        int numStocks = p.Holdings.Count;
        return numStocks > 5 ? 0.3 : 0.5;
    }
}

class PortfolioManager
{
    public List<Investor> Investors { get; } = new List<Investor>();
    public List<Stock> Stocks { get; } = new List<Stock>();
    public List<Transaction> Transactions { get; } = new List<Transaction>();
    public Dictionary<string, List<Transaction>> TransactionsByStock { get; } = new Dictionary<string, List<Transaction>>();
    public List<Portfolio> Portfolios { get; } = new List<Portfolio>();
    Dictionary<string, Stock> _stockDict = new Dictionary<string, Stock>();
    IRiskStrategy _riskStrategy = new SimpleRiskStrategy();

    public void AddTransaction(int investorId, string symbol, int qty, double price, bool isBuy, DateTime? date = null)
    {
        var dt = date ?? DateTime.Now;
        if (dt > DateTime.Now) throw new InvalidTradeException("Transaction date cannot be in future");
        if (isBuy == false)
        {
            var port = Portfolios.FirstOrDefault(p => p.InvestorId == investorId);
            if (port == null || !port.Holdings.ContainsKey(symbol) || port.Holdings[symbol] < qty)
                throw new InvalidTradeException("Cannot sell more than owned");
        }
        var t = new Transaction { Id = Transactions.Count + 1, InvestorId = investorId, StockSymbol = symbol, Quantity = qty, PricePerShare = price, Date = dt, IsBuy = isBuy };
        Transactions.Add(t);
        if (!TransactionsByStock.ContainsKey(symbol)) TransactionsByStock[symbol] = new List<Transaction>();
        TransactionsByStock[symbol].Add(t);

        var p2 = Portfolios.FirstOrDefault(p => p.InvestorId == investorId);
        if (p2 == null) { p2 = new Portfolio { InvestorId = investorId }; Portfolios.Add(p2); }
        if (isBuy) p2.Holdings[symbol] = p2.Holdings.GetValueOrDefault(symbol) + qty;
        else p2.Holdings[symbol] = p2.Holdings[symbol] - qty;
    }

    public double GetPortfolioRisk(int investorId)
    {
        var port = Portfolios.FirstOrDefault(p => p.InvestorId == investorId);
        if (port == null) return 0;
        return _riskStrategy.CalculateRisk(port, _stockDict);
    }

    public void RegisterStocks()
    {
        foreach (var s in Stocks) _stockDict[s.Symbol] = s;
    }
}

class Program
{
    static PortfolioManager mgr = new PortfolioManager();

    static void Main(string[] args)
    {
        mgr.Stocks.Add(new Stock { Symbol = "ABC", Name = "ABC Ltd", Price = 100 });
        mgr.Stocks.Add(new Stock { Symbol = "XYZ", Name = "XYZ Inc", Price = 200 });
        mgr.RegisterStocks();

        mgr.Investors.Add(new Investor { Id = 1, Name = "Rahul" });
        mgr.Investors.Add(new Investor { Id = 2, Name = "Priya" });

        mgr.AddTransaction(1, "ABC", 10, 100, true);
        mgr.AddTransaction(1, "ABC", 5, 110, false);
        mgr.AddTransaction(1, "XYZ", 20, 200, true);
        mgr.AddTransaction(2, "ABC", 50, 95, true);
        mgr.AddTransaction(2, "XYZ", 10, 210, true);

        var abc = mgr.Stocks.First(s => s.Symbol == "ABC");
        abc.PriceChanged += (stock, oldPrice) => Console.WriteLine($"[Event] {stock.Symbol} price changed from {oldPrice} to {stock.Price}");

        while (true)
        {
            Console.WriteLine("\n--- Stock Portfolio ---");
            Console.WriteLine("1. Buy  2. Sell  3. Change price (event)  4. Portfolio risk");
            Console.WriteLine("5. Most profitable investor  6. Highest volume stock  7. Group by stock  8. Net P/L  9. Negative return investors  0. Exit");
            string c = Console.ReadLine();
            if (c == "0") break;
            try
            {
                if (c == "1") DoTrade(true);
                else if (c == "2") DoTrade(false);
                else if (c == "3") { Console.Write("Symbol and new price: "); var s = Console.ReadLine().Split(); mgr.Stocks.First(x => x.Symbol == s[0]).UpdatePrice(double.Parse(s[1])); }
                else if (c == "4") { Console.Write("InvestorId: "); Console.WriteLine("Risk: " + mgr.GetPortfolioRisk(int.Parse(Console.ReadLine()))); }
                else if (c == "5") LinqMostProfitable();
                else if (c == "6") LinqHighestVolume();
                else if (c == "7") LinqGroupByStock();
                else if (c == "8") LinqNetPL();
                else if (c == "9") LinqNegativeReturns();
            }
            catch (Exception ex) { Console.WriteLine("Error: " + ex.Message); }
        }
    }

    static void DoTrade(bool isBuy)
    {
        Console.Write("InvestorId Symbol Qty Price: ");
        var s = Console.ReadLine().Split();
        mgr.AddTransaction(int.Parse(s[0]), s[1], int.Parse(s[2]), double.Parse(s[3]), isBuy);
        Console.WriteLine(isBuy ? "Bought" : "Sold");
    }

    static void LinqMostProfitable()
    {
        var pnl = mgr.Transactions.GroupBy(t => t.InvestorId).Select(g =>
        {
            double buy = g.Where(t => t.IsBuy).Sum(t => t.Quantity * t.PricePerShare);
            double sell = g.Where(t => !t.IsBuy).Sum(t => t.Quantity * t.PricePerShare);
            return new { InvestorId = g.Key, Profit = sell - buy };
        }).OrderByDescending(x => x.Profit).First();
        var inv = mgr.Investors.First(i => i.Id == pnl.InvestorId);
        Console.WriteLine("Most profitable: " + inv.Name + " profit " + pnl.Profit);
    }

    static void LinqHighestVolume()
    {
        var vol = mgr.TransactionsByStock.Select(kv => new { Symbol = kv.Key, Vol = kv.Value.Sum(t => t.Quantity) }).OrderByDescending(x => x.Vol).First();
        Console.WriteLine("Highest volume: " + vol.Symbol + " volume " + vol.Vol);
    }

    static void LinqGroupByStock()
    {
        foreach (var kv in mgr.TransactionsByStock)
            Console.WriteLine(kv.Key + ": " + kv.Value.Count + " transactions");
    }

    static void LinqNetPL()
    {
        double totalBuy = mgr.Transactions.Where(t => t.IsBuy).Aggregate(0.0, (sum, t) => sum + t.Quantity * t.PricePerShare);
        double totalSell = mgr.Transactions.Where(t => !t.IsBuy).Aggregate(0.0, (sum, t) => sum + t.Quantity * t.PricePerShare);
        double net = totalSell - totalBuy;
        Console.WriteLine("Net P/L: " + net);
    }

    static void LinqNegativeReturns()
    {
        var neg = mgr.Transactions.GroupBy(t => t.InvestorId).Select(g =>
        {
            double buy = g.Where(t => t.IsBuy).Sum(t => t.Quantity * t.PricePerShare);
            double sell = g.Where(t => !t.IsBuy).Sum(t => t.Quantity * t.PricePerShare);
            return new { InvestorId = g.Key, Profit = sell - buy };
        }).Where(x => x.Profit < 0);
        foreach (var x in neg)
        {
            var inv = mgr.Investors.First(i => i.Id == x.InvestorId);
            Console.WriteLine(inv.Name + " return: " + x.Profit);
        }
    }
}
