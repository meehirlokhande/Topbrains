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
                service.AddViolation(new Violation(parts[0], parts[1], int.Parse(parts[2])));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            while (true)
            {
                Console.WriteLine("1 -> Display Violations");
                Console.WriteLine("2 -> Pay Fine");
                Console.WriteLine("3 -> Add Violation");
                Console.WriteLine("4 -> Exit");

                int choice = int.Parse(Console.ReadLine());

                try
                {
                    switch (choice)
                    {
                        case 1:
                            service.GetAllViolations();
                            break;
                        case 2:
                            string payVeh = Console.ReadLine();
                            int payAmt = int.Parse(Console.ReadLine());
                            service.PayFine(payVeh, payAmt);
                            break;
                        case 3:
                            string addInput = Console.ReadLine();
                            string[] addParts = addInput.Split(' ');
                            service.AddViolation(new Violation(addParts[0], addParts[1], int.Parse(addParts[2])));
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
