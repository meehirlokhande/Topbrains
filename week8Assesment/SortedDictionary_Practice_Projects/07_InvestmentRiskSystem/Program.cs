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
                service.AddInvestment(new Investment(parts[0], parts[1], int.Parse(parts[2])));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            while (true)
            {
                Console.WriteLine("1 -> Display Investments");
                Console.WriteLine("2 -> Update Risk");
                Console.WriteLine("3 -> Add Investment");
                Console.WriteLine("4 -> Exit");

                int choice = int.Parse(Console.ReadLine());

                try
                {
                    switch (choice)
                    {
                        case 1:
                            service.GetAllInvestments();
                            break;
                        case 2:
                            string updateId = Console.ReadLine();
                            int newRisk = int.Parse(Console.ReadLine());
                            service.UpdateRisk(updateId, newRisk);
                            break;
                        case 3:
                            string addInput = Console.ReadLine();
                            string[] addParts = addInput.Split(' ');
                            service.AddInvestment(new Investment(addParts[0], addParts[1], int.Parse(addParts[2])));
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
