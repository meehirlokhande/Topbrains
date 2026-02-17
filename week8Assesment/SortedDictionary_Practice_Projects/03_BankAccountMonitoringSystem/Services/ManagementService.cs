using System;
using System.Collections.Generic;
using Domain;
using Exceptions;

namespace Services
{
    public class ManagementService
    {
        private SortedDictionary<decimal, List<Account>> _data = new SortedDictionary<decimal, List<Account>>();

        public void AddAccount(Account account)
        {
            if (account.Balance < 0)
                throw new NegativeBalanceException("Negative Balance");

            foreach (var kvp in _data)
            {
                foreach (var a in kvp.Value)
                {
                    if (a.AccountNumber == account.AccountNumber)
                        throw new AccountNotFoundException("Duplicate Account");
                }
            }

            if (!_data.ContainsKey(account.Balance))
                _data[account.Balance] = new List<Account>();

            _data[account.Balance].Add(account);
        }

        public void GetAllAccounts()
        {
            foreach (var kvp in _data)
            {
                foreach (var a in kvp.Value)
                {
                    Console.WriteLine($"Details: {a.AccountNumber} {a.HolderName} {a.Balance}");
                }
            }
        }

        public void Deposit(string accountNumber, decimal amount)
        {
            if (amount <= 0)
                throw new NegativeBalanceException("Invalid Deposit Amount");

            foreach (var kvp in _data)
            {
                foreach (var a in kvp.Value)
                {
                    if (a.AccountNumber == accountNumber)
                    {
                        decimal oldBalance = a.Balance;
                        kvp.Value.Remove(a);
                        if (kvp.Value.Count == 0)
                            _data.Remove(oldBalance);

                        a.Balance += amount;

                        if (!_data.ContainsKey(a.Balance))
                            _data[a.Balance] = new List<Account>();
                        _data[a.Balance].Add(a);

                        Console.WriteLine($"Updated Balance: {a.Balance}");
                        return;
                    }
                }
            }
            throw new AccountNotFoundException("Account Not Found");
        }

        public void Withdraw(string accountNumber, decimal amount)
        {
            if (amount <= 0)
                throw new NegativeBalanceException("Invalid Withdrawal Amount");

            foreach (var kvp in _data)
            {
                foreach (var a in kvp.Value)
                {
                    if (a.AccountNumber == accountNumber)
                    {
                        if (a.Balance < amount)
                            throw new InsufficientFundsException("Insufficient Funds");

                        decimal oldBalance = a.Balance;
                        kvp.Value.Remove(a);
                        if (kvp.Value.Count == 0)
                            _data.Remove(oldBalance);

                        a.Balance -= amount;

                        if (!_data.ContainsKey(a.Balance))
                            _data[a.Balance] = new List<Account>();
                        _data[a.Balance].Add(a);

                        Console.WriteLine($"Updated Balance: {a.Balance}");
                        return;
                    }
                }
            }
            throw new AccountNotFoundException("Account Not Found");
        }
    }
}
