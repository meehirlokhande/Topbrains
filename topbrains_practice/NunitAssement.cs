using System;
using NUnit.Framework;


public class Program
{
    public decimal Balance { get; private set; }

    
    public Program(decimal initialBalance)
    {
        Balance = initialBalance;
    }

    
    public void Deposit(decimal amount)
    {
        if (amount < 0)
        {
            throw new Exception("Deposit amount cannot be negative");
        }

        Balance += amount;
    }

    
    public void Withdraw(decimal amount)
    {
        if (amount > Balance)
        {
            throw new Exception("Insufficient funds.");
        }

        Balance -= amount;
    }

    
    public static void Main(string[] args)
    {
        
        
    }
}

// NUnit Test Class
[TestFixture]
public class UnitTest
{
    [Test]
    public void Test_Deposit_ValidAmount()
    {
        Program account = new Program(1000);
        account.Deposit(500);

        Assert.AreEqual(1500, account.Balance);
    }

    [Test]
    public void Test_Deposit_NegativeAmount()
    {
        Program account = new Program(1000);

        Assert.AreEqual(
            "Deposit amount cannot be negative",
            Assert.Throws<Exception>(() => account.Deposit(-200)).Message
        );
    }

    [Test]
    public void Test_Withdraw_ValidAmount()
    {
        Program account = new Program(1000);
        account.Withdraw(400);

        Assert.AreEqual(600, account.Balance);
    }

    [Test]
    public void Test_Withdraw_InsufficientFunds()
    {
        Program account = new Program(500);

        Assert.AreEqual(
            "Insufficient funds.",
            Assert.Throws<Exception>(() => account.Withdraw(800)).Message
        );
    }
}
