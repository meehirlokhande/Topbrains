using System;
using System.Collections.Generic;
using System.Linq;

public interface IFinancialInstrument
{
    string Symbol { get; }
    decimal CurrentPrice { get; }
    InstrumentType Type { get; }
}


public enum InstrumentType { Stock, Bond, Option, Future }


public enum Trend { Upward, Downward, Sideways }


public class Portfolio<T> where T : IFinancialInstrument
{
    private Dictionary<T, int> _holdings = new();


    public IReadOnlyDictionary<T, int> Holdings => _holdings;


    public void Buy(T instrument, int quantity, decimal price)
    {
        if (quantity <= 0 || price <= 0)
            throw new ArgumentException("Quantity and price must be positive.");


        if (_holdings.ContainsKey(instrument))
            _holdings[instrument] += quantity;
        else
            _holdings[instrument] = quantity;


        Console.WriteLine($"Bought {quantity} of {instrument.Symbol}");
    }

    public decimal? Sell(T instrument, int quantity, decimal currentPrice)
    {
        if (!_holdings.ContainsKey(instrument) || _holdings[instrument] < quantity)
        {
            Console.WriteLine("Sell failed: Not enough holdings."); return null;
        }


        _holdings[instrument] -= quantity;


        if (_holdings[instrument] == 0)
            _holdings.Remove(instrument);


        decimal proceeds = quantity * currentPrice; Console.WriteLine($"Sold {quantity} of {instrument.Symbol}"); return proceeds;
    }


    public decimal CalculateTotalValue()
    {
        return _holdings.Sum(h => h.Key.CurrentPrice * h.Value);
    }


    public (T instrument, decimal returnPercentage)? GetTopPerformer(Dictionary<T, decimal> purchasePrices)
    {
        if (!purchasePrices.Any()) return null;


        var result = _holdings
        .Where(h => purchasePrices.ContainsKey(h.Key))
        .Select(h =>
        {
            var purchasePrice = purchasePrices[h.Key]; var returnPercent =
    ((h.Key.CurrentPrice - purchasePrice) / purchasePrice) * 100; return (instrument: h.Key, returnPercentage: returnPercent);
        })
        .OrderByDescending(r => r.returnPercentage)
        .FirstOrDefault();


        return result.instrument != null ? result : null;

    }
}


public class Stock : IFinancialInstrument
{
    public string Symbol { get; set; }
    public decimal CurrentPrice { get; set; }
    public InstrumentType Type => InstrumentType.Stock; public string CompanyName { get; set; }
    public decimal DividendYield { get; set; }
}


public class Bond : IFinancialInstrument
{
    public string Symbol { get; set; }
    public decimal CurrentPrice { get; set; }
    public InstrumentType Type => InstrumentType.Bond; public DateTime MaturityDate { get; set; }
    public decimal CouponRate { get; set; }
}


public class TradingStrategy<T> where T : IFinancialInstrument
{
    public void Execute(Portfolio<T> portfolio, IEnumerable<T> marketData, Func<T, bool> buyCondition, Func<T, bool> sellCondition)
    {
        foreach (var instrument in marketData)
        {
            if (buyCondition(instrument))
                portfolio.Buy(instrument, 10, instrument.CurrentPrice);


            if (sellCondition(instrument) && portfolio.Holdings.ContainsKey(instrument)) portfolio.Sell(instrument, 5, instrument.CurrentPrice);
        }
    }


    public Dictionary<string, decimal> CalculateRiskMetrics(IEnumerable<T> instruments)

    {
        var prices = instruments.Select(i => i.CurrentPrice).ToList(); if (!prices.Any())
            return new Dictionary<string, decimal>();


        decimal avg = prices.Average();
        decimal variance = prices.Average(p => (p - avg) * (p - avg)); decimal volatility = (decimal)Math.Sqrt((double)variance);

        decimal beta = volatility / (avg == 0 ? 1 : avg); decimal sharpe = avg == 0 ? 0 : (avg - 2) / volatility;

        return new Dictionary<string, decimal>
{
{ "Volatility", volatility },
{ "Beta", beta },
{ "SharpeRatio", sharpe }
};
    }
}


public class PriceHistory<T> where T : IFinancialInstrument
{
    private Dictionary<T, List<(DateTime, decimal)>> _history = new();


    public void AddPrice(T instrument, DateTime timestamp, decimal price)
    {
        if (!_history.ContainsKey(instrument))
            _history[instrument] = new List<(DateTime, decimal)>();


        _history[instrument].Add((timestamp, price));
    }


    public decimal? GetMovingAverage(T instrument, int days)
    {
        if (!_history.ContainsKey(instrument)) return null;

        var recent = _history[instrument]
        .OrderByDescending(p => p.Item1)
        .Take(days)

        .Select(p => p.Item2)
        .ToList();


        if (!recent.Any()) return null;

        return recent.Average();
    }


    public Trend DetectTrend(T instrument, int period)
    {
        if (!_history.ContainsKey(instrument)) return Trend.Sideways;

        var recent = _history[instrument]
        .OrderByDescending(p => p.Item1)
        .Take(period)
        .Select(p => p.Item2)
        .Reverse()
        .ToList();


        if (recent.Count < 2) return Trend.Sideways;

        if (recent.Last() > recent.First()) return Trend.Upward;

        if (recent.Last() < recent.First()) return Trend.Downward;

        return Trend.Sideways;
    }
}


public class Program
{
    public static void Main()
    {
        var stock1 = new Stock
        {
            Symbol = "AAPL",

            CurrentPrice = 150,
            CompanyName = "Apple",
            DividendYield = 0.5m
        };


        var stock2 = new Stock
        {
            Symbol = "MSFT",
            CurrentPrice = 300,
            CompanyName = "Microsoft",
            DividendYield = 0.8m
        };


        var bond1 = new Bond
        {
            Symbol = "GOVT10Y",
            CurrentPrice = 100,
            CouponRate = 5,
            MaturityDate = DateTime.Now.AddYears(10)
        };


        var portfolio = new Portfolio<IFinancialInstrument>();


        portfolio.Buy(stock1, 20, stock1.CurrentPrice);
        portfolio.Buy(stock2, 10, stock2.CurrentPrice);
        portfolio.Buy(bond1, 50, bond1.CurrentPrice);


        Console.WriteLine("Total Portfolio Value: " + portfolio.CalculateTotalValue());


        var purchasePrices = new Dictionary<IFinancialInstrument, decimal>
{
{ stock1, 120 },
{ stock2, 250 },
{ bond1, 95 }
};


        var top = portfolio.GetTopPerformer(purchasePrices);
        Console.WriteLine($"Top Performer: {top?.instrument.Symbol} {top?.returnPercentage:F2}%");


        var strategy = new TradingStrategy<IFinancialInstrument>();

        strategy.Execute(portfolio,
        new List<IFinancialInstrument> { stock1, stock2, bond1 }, i => i.CurrentPrice < 200,
        i => i.CurrentPrice > 250
        );


        Console.WriteLine("After Strategy Value: " + portfolio.CalculateTotalValue());


        var history = new PriceHistory<IFinancialInstrument>(); history.AddPrice(stock1, DateTime.Now.AddDays(-3), 140);
        history.AddPrice(stock1, DateTime.Now.AddDays(-2), 145);
        history.AddPrice(stock1, DateTime.Now.AddDays(-1), 150);


        Console.WriteLine("Moving Avg: " + history.GetMovingAverage(stock1, 3)); Console.WriteLine("Trend: " + history.DetectTrend(stock1, 3));

        var risk = strategy.CalculateRiskMetrics(new List<IFinancialInstrument>
{
stock1, stock2, bond1
});


        foreach (var metric in risk) Console.WriteLine($"{metric.Key}: {metric.Value:F2}");
    }
}
