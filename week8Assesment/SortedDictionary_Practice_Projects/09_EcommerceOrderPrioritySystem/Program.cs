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
                service.AddOrder(new Order(parts[0], parts[1], int.Parse(parts[2])));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            while (true)
            {
                Console.WriteLine("1 -> Display Orders");
                Console.WriteLine("2 -> Update Order");
                Console.WriteLine("3 -> Add Order");
                Console.WriteLine("4 -> Exit");

                int choice = int.Parse(Console.ReadLine());

                try
                {
                    switch (choice)
                    {
                        case 1:
                            service.GetAllOrders();
                            break;
                        case 2:
                            string updateId = Console.ReadLine();
                            int newAmt = int.Parse(Console.ReadLine());
                            service.UpdateOrder(updateId, newAmt);
                            break;
                        case 3:
                            string addInput = Console.ReadLine();
                            string[] addParts = addInput.Split(' ');
                            service.AddOrder(new Order(addParts[0], addParts[1], int.Parse(addParts[2])));
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
