using System;
using System.Collections.Generic;
using System.Linq;

// Custom Exception Classes
public class InsufficientBalanceException : Exception
{
    public InsufficientBalanceException(string message) : base(message) { }
}

public class MinimumBalanceException : Exception
{
    public MinimumBalanceException(string message) : base(message) { }
}

public class InvalidTransactionException : Exception
{
    public InvalidTransactionException(string message) : base(message) { }
}


public abstract class BankAccount
{
    public int AccountNumber { get; set; }
    public string Name { get; set; }
    public decimal Balance { get; set; }

    public BankAccount(int accountNumber, string customerName, decimal balance)
    {
        this.AccountNumber = accountNumber;
        this.Name = customerName;
        this.Balance = balance;
    }

    public virtual void Deposit(decimal amount)
    {
        if (amount <= 0)
        {
            throw new InvalidTransactionException("Deposit amount must be positive.");
        }
        Balance += amount;
        Console.WriteLine($"Deposited {amount}. New balance is {Balance}");
    }

    public virtual void Withdraw(decimal amount)
    {
        if (amount < 0)
        {
            throw new InvalidTransactionException("Withdrawal amount must be positive.");
        }
        if (amount > Balance)
        {
            throw new InsufficientBalanceException($"Cannot Withdraw Amount more than the balance.");
        }
        Balance -= amount;
        Console.WriteLine($"Withdrew {amount} New Balance {Balance}");
    }


    public abstract void CalculateInterest();


}

public class SavingAccount : BankAccount
{
    private const decimal MinimumBalance = 1000m;
    private const decimal InterestRate = 0.04m;

    public SavingAccount(int accountNumber, string customerName, decimal balance) : base(accountNumber, customerName, balance) { }


    public override void Withdraw(decimal amount)
    {
        if (amount <= 0)
        {
            throw new InvalidTransactionException("Withdrawal amount must be positive.");
        }
        if (amount > Balance)
        {
            throw new InsufficientBalanceException("withdrawal amount cannot be greater than balance.");
        }

        if ((Balance - amount) < MinimumBalance)
        {
            throw new MinimumBalanceException($"Cannot withdraw .Balance will drop below minimum limit.");
        }

        Balance -= amount;
        Console.WriteLine($" Withdrew {amount} New Balance : {Balance}");
    }

    public override void CalculateInterest()
    {
        decimal interest = Balance * InterestRate;
        Balance += interest;
        Console.WriteLine($"Interest of {interest} added New Balance: {Balance}");
    }
}

public class CurrentAccount : BankAccount
{
    public decimal OverdraftLimit { get; set; }

    public CurrentAccount(int accountNumber, string customerName, decimal balance, decimal OverdraftLimit = 10000m) : base(accountNumber, customerName, balance)
    {
        OverdraftLimit = OverdraftLimit;
    }

    public override void Withdraw(decimal amount)
    {
        if (amount <= 0)
        {
            throw new InvalidTransactionException("Withdrawal amount  must be positive.");
        }

        if (amount > (Balance + OverdraftLimit))
        {
            throw new InsufficientBalanceException($"Cannot Withdraw {amount} Exceeedss Overdraft limit.");
        }

        Balance -= amount;
        Console.WriteLine($"Withdraw {amount} New Balance: {Balance}");
    }

    public override void CalculateInterest()
    {
        Console.WriteLine("  Current accounts earn no interest.");
    }

}

public class LoanAccount : BankAccount
{
    private const decimal InterestRate = 0.08m; // 8%

    public LoanAccount(int accountNumber, string customerName, decimal loanBalance)
        : base(accountNumber, customerName, loanBalance) { }

    // Deposits are not allowed on loan accounts
    public override void Deposit(decimal amount)
    {
        throw new InvalidTransactionException(
            "Deposit is not allowed on a Loan Account. Use Withdraw (repay) instead.");
    }

    // Withdraw here represents loan repayment
    public override void Withdraw(decimal amount)
    {
        if (amount <= 0)
            throw new InvalidTransactionException("Repayment amount must be positive.");

        Balance -= amount;
        Console.WriteLine($"  Repaid {amount:C}. Remaining Loan: {Balance:C}");
    }

    public override void CalculateInterest()
    {
        decimal interest = Balance * InterestRate;
        Balance += interest;
        Console.WriteLine($"  Interest of {interest:C} charged (8%). New Loan Balance: {Balance:C}");
    }
}

class Program
{
    public static void Main(string[] args)
    {
        List<BankAccount> accounts = new List<BankAccount>
        {
            new SavingAccount(101,"Rahul",55500m)
        };

        foreach (var acc in accounts)
        {
            Console.WriteLine(acc);
        }

        try
        {
            Console.WriteLine("\nDepositing 5000 into Rahul's Savings:");
            accounts[0].Deposit(5000m);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"  ERROR: {ex.Message}");
        }

        // Withdraw that would violate minimum balance
        try
        {
            Console.WriteLine("\nWithdrawing 2500 from Amit's Savings (balance 3000, min 1000):");
            accounts[2].Withdraw(2500m);
        }
        catch (MinimumBalanceException ex)
        {
            Console.WriteLine($"  ERROR: {ex.Message}");
        }
        // CurrentAccount overdraft withdrawal
        try
        {
            Console.WriteLine("\nWithdrawing 130000 from Rajesh's Current (balance 120000, overdraft 20000):");
            accounts[3].Withdraw(130000m);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"  ERROR: {ex.Message}");
        }

        // Deposit into LoanAccount (should throw)
        try
        {
            Console.WriteLine("\nDepositing into Rohan's Loan Account:");
            accounts[5].Deposit(5000m);
        }
        catch (InvalidTransactionException ex)
        {
            Console.WriteLine($"  ERROR: {ex.Message}");
        }

    }
}