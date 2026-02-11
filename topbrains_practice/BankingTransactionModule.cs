using System;
class Account{
    public string AccountNumber { get; set; }
    public decimal Balance { get; set; }

    public Account(string accountNumber, decimal balance){
        AccountNumber = accountNumber;
        Balance = balance;
    }

    public decimal Deposit(decimal amount){
        try{
            if(amount < 0){
                throw new ArgumentException("Deposit amount must be positive.");
            }
            Balance += amount;
            
        }catch(Exception e){
             Console.WriteLine("Error: " + e.Message);
        }

        return Balance;
    }

    public decimal Withdraw(decimal amount){
        try{
            if(amount < 0){
                throw new ArgumentException("Withdrawal amount must be positive.");
            }
            if(amount > Balance){
                throw new InvalidOperationException("Insufficient funds.");
            }
            Balance -= amount;
        }
        catch(ArgumentException e){
            Console.WriteLine("Error: "+ e.Message);
        }
        catch(InvalidOperationException e){
            Console.WriteLine("Error: "+ e.Message);
        }
        return Balance;
    }
}


class BankingTransactionModule{
    public static void Main(string[] args){

        Console.WriteLine("Press 1 to deposit");
        Console.WriteLine("Press 2 to withdraw");
        Console.WriteLine("Press 3 to exit");
        
        while(true){
            int choice = int.Parse(Console.ReadLine());
            switch(choice){
                case 1:
                    Console.WriteLine("Enter the amount to deposit");
                    decimal amount = decimal.Parse(Console.ReadLine());
                    account.Deposit(amount);
                    break;
                case 2:
                    Console.WriteLine("Enter the amount to withdraw");
                    amount = decimal.Parse(Console.ReadLine());
                    account.Withdraw(amount);
                    break;
                case 3:
                    return;
                default:
                    Console.WriteLine("Invalid choice");
                    break;
            }
        }

    }
}   