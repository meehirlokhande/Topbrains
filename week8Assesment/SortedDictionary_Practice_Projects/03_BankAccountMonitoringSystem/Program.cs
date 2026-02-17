using System;
using Domain;
using Services;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            ManagementService service = new ManagementService();

            string input = Console.ReadLine();
            string[] parts = input.Split(' ');
            try
            {
                service.AddAccount(new Account(parts[0], parts[1], decimal.Parse(parts[2])));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            while (true)
            {
                Console.WriteLine("1 -> Display Accounts");
                Console.WriteLine("2 -> Deposit");
                Console.WriteLine("3 -> Withdraw");
                Console.WriteLine("4 -> Exit");

                int choice = int.Parse(Console.ReadLine());

                try
                {
                    switch (choice)
                    {
                        case 1:
                            service.GetAllAccounts();
                            break;
                        case 2:
                            string depAcc = Console.ReadLine();
                            decimal depAmt = decimal.Parse(Console.ReadLine());
                            service.Deposit(depAcc, depAmt);
                            break;
                        case 3:
                            string wdAcc = Console.ReadLine();
                            decimal wdAmt = decimal.Parse(Console.ReadLine());
                            service.Withdraw(wdAcc, wdAmt);
                            break;
                        case 4:
                            Console.WriteLine("Thank You");
                            return;
                        default:
                            Console.WriteLine("Invalid choice");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
