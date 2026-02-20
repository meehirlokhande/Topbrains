#nullable disable
// Custom exceptions
class InsufficientBalanceException : Exception
{
    public InsufficientBalanceException(string msg) : base(msg) { }
}
class MinimumBalanceException : Exception
{
    public MinimumBalanceException(string msg) : base(msg) { }
}
class InvalidTransactionException : Exception
{
    public InvalidTransactionException(string msg) : base(msg) { }
}

abstract class BankAccount
{
    public string AccountNumber { get; set; }
    public string CustomerName { get; set; }
    public double Balance { get; set; }
    public List<string> TransactionHistory { get; } = new List<string>();

    protected BankAccount(string accNo, string name, double balance)
    {
        AccountNumber = accNo;
        CustomerName = name;
        Balance = balance;
    }

    public virtual void Deposit(double amount)
    {
        if (amount <= 0) throw new InvalidTransactionException("Amount must be positive");
        Balance += amount;
        TransactionHistory.Add($"{DateTime.Now:yyyy-MM-dd HH:mm} Deposit +{amount}. Balance: {Balance}");
    }

    public virtual void Withdraw(double amount)
    {
        if (amount <= 0) throw new InvalidTransactionException("Amount must be positive");
        if (amount > Balance) throw new InsufficientBalanceException("Not enough balance");
        Balance -= amount;
        TransactionHistory.Add($"{DateTime.Now:yyyy-MM-dd HH:mm} Withdraw -{amount}. Balance: {Balance}");
    }

    public abstract double CalculateInterest();
}

class SavingsAccount : BankAccount
{
    const double MinBalance = 1000;
    const double InterestRate = 0.04;

    public SavingsAccount(string accNo, string name, double balance) : base(accNo, name, balance) { }

    public override void Withdraw(double amount)
    {
        if (Balance - amount < MinBalance)
            throw new MinimumBalanceException($"Savings must keep min {MinBalance}");
        base.Withdraw(amount);
    }

    public override double CalculateInterest() => Balance * InterestRate;
}

class CurrentAccount : BankAccount
{
    public double OverdraftLimit { get; set; }

    public CurrentAccount(string accNo, string name, double balance, double overdraft = 5000)
        : base(accNo, name, balance) { OverdraftLimit = overdraft; }

    public override void Withdraw(double amount)
    {
        if (amount <= 0) throw new InvalidTransactionException("Amount must be positive");
        if (Balance - amount < -OverdraftLimit)
            throw new InsufficientBalanceException($"Cannot exceed overdraft limit {OverdraftLimit}");
        Balance -= amount;
        TransactionHistory.Add($"{DateTime.Now:yyyy-MM-dd HH:mm} Withdraw -{amount}. Balance: {Balance}");
    }

    public override double CalculateInterest() => 0; // no interest for current
}

class LoanAccount : BankAccount
{
    const double InterestRate = 0.12;

    public LoanAccount(string accNo, string name, double balance) : base(accNo, name, balance) { }

    public override void Deposit(double amount)
    {
        throw new InvalidTransactionException("Loan account does not allow deposit");
    }

    public override double CalculateInterest() => Balance * InterestRate;
}

class Bank
{
    public List<BankAccount> Accounts { get; } = new List<BankAccount>();

    public void Transfer(BankAccount from, BankAccount to, double amount)
    {
        if (from is LoanAccount) throw new InvalidTransactionException("Cannot transfer from loan account");
        if (to is LoanAccount) throw new InvalidTransactionException("Cannot transfer to loan account");
        from.Withdraw(amount);
        to.Deposit(amount);
        from.TransactionHistory.Add($"{DateTime.Now:yyyy-MM-dd HH:mm} Transfer out -{amount} to {to.AccountNumber}");
        to.TransactionHistory.Add($"{DateTime.Now:yyyy-MM-dd HH:mm} Transfer in +{amount} from {from.AccountNumber}");
    }
}

class Program
{
    static Bank bank = new Bank();

    static void Main(string[] args)
    {
        // sample data
        bank.Accounts.Add(new SavingsAccount("S001", "Rahul", 60000));
        bank.Accounts.Add(new SavingsAccount("S002", "Raj", 30000));
        bank.Accounts.Add(new CurrentAccount("C001", "Ravi", 25000, 10000));
        bank.Accounts.Add(new LoanAccount("L001", "Priya", 100000));

        while (true)
        {
            Console.WriteLine("\n--- Smart Banking ---");
            Console.WriteLine("1. Deposit  2. Withdraw  3. Transfer  4. Calculate Interest");
            Console.WriteLine("5. LINQ: Balance > 50k  6. Total balance  7. Top 3 balance  8. Group by type  9. Name starts with R");
            Console.WriteLine("10. Show history  0. Exit");
            string choice = Console.ReadLine();
            if (choice == "0") break;
            try
            {
                if (choice == "1") DoDeposit();
                else if (choice == "2") DoWithdraw();
                else if (choice == "3") DoTransfer();
                else if (choice == "4") DoInterest();
                else if (choice == "5") LinqBalanceAbove50k();
                else if (choice == "6") LinqTotalBalance();
                else if (choice == "7") LinqTop3();
                else if (choice == "8") LinqGroupByType();
                else if (choice == "9") LinqNameStartsWithR();
                else if (choice == "10") ShowHistory();
            }
            catch (Exception ex) { Console.WriteLine("Error: " + ex.Message); }
        }
    }

    static BankAccount FindAccount(string prompt)
    {
        Console.Write(prompt);
        string acc = Console.ReadLine();
        var a = bank.Accounts.FirstOrDefault(x => x.AccountNumber == acc);
        if (a == null) throw new InvalidTransactionException("Account not found");
        return a;
    }

    static void DoDeposit()
    {
        var acc = FindAccount("Account number: ");
        Console.Write("Amount: ");
        double amt = double.Parse(Console.ReadLine());
        acc.Deposit(amt);
        Console.WriteLine("Deposit done. New balance: " + acc.Balance);
    }

    static void DoWithdraw()
    {
        var acc = FindAccount("Account number: ");
        Console.Write("Amount: ");
        double amt = double.Parse(Console.ReadLine());
        acc.Withdraw(amt);
        Console.WriteLine("Withdraw done. New balance: " + acc.Balance);
    }

    static void DoTransfer()
    {
        var from = FindAccount("From account: ");
        var to = FindAccount("To account: ");
        Console.Write("Amount: ");
        double amt = double.Parse(Console.ReadLine());
        bank.Transfer(from, to, amt);
        Console.WriteLine("Transfer done.");
    }

    static void DoInterest()
    {
        foreach (var acc in bank.Accounts)
        {
            double interest = acc.CalculateInterest();
            Console.WriteLine($"{acc.AccountNumber} ({acc.GetType().Name}): Interest = {interest:F2}");
        }
    }

    static void LinqBalanceAbove50k()
    {
        var list = bank.Accounts.Where(a => a.Balance > 50000).ToList();
        foreach (var a in list) Console.WriteLine($"{a.AccountNumber} {a.CustomerName} {a.Balance}");
    }

    static void LinqTotalBalance()
    {
        double total = bank.Accounts.Sum(a => a.Balance);
        Console.WriteLine("Total bank balance: " + total);
    }

    static void LinqTop3()
    {
        var top = bank.Accounts.OrderByDescending(a => a.Balance).Take(3);
        foreach (var a in top) Console.WriteLine($"{a.AccountNumber} {a.CustomerName} {a.Balance}");
    }

    static void LinqGroupByType()
    {
        var groups = bank.Accounts.GroupBy(a => a.GetType().Name);
        foreach (var g in groups)
        {
            Console.WriteLine(g.Key + ":");
            foreach (var a in g) Console.WriteLine("  " + a.AccountNumber + " " + a.CustomerName);
        }
    }

    static void LinqNameStartsWithR()
    {
        var list = bank.Accounts.Where(a => a.CustomerName.StartsWith("R", StringComparison.OrdinalIgnoreCase));
        foreach (var a in list) Console.WriteLine($"{a.AccountNumber} {a.CustomerName}");
    }

    static void ShowHistory()
    {
        var acc = FindAccount("Account number: ");
        foreach (var t in acc.TransactionHistory) Console.WriteLine(t);
    }
}
