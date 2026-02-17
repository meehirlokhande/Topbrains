using System;
using Domain;
using Services;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            MedicineUtility service = new MedicineUtility();

            string input = Console.ReadLine();
            string[] parts = input.Split(' ');
            try
            {
                service.AddMedicine(new Medicine(parts[0], parts[1], int.Parse(parts[2]), int.Parse(parts[3])));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            while (true)
            {
                Console.WriteLine("1 -> Display all medicines");
                Console.WriteLine("2 -> Update medicine price");
                Console.WriteLine("3 -> Add medicine");
                Console.WriteLine("4 -> Exit");

                int choice = int.Parse(Console.ReadLine());

                try
                {
                    switch (choice)
                    {
                        case 1:
                            service.GetAllMedicines();
                            break;
                        case 2:
                            string updateId = Console.ReadLine();
                            int newPrice = int.Parse(Console.ReadLine());
                            service.UpdateMedicinePrice(updateId, newPrice);
                            break;
                        case 3:
                            string addInput = Console.ReadLine();
                            string[] addParts = addInput.Split(' ');
                            service.AddMedicine(new Medicine(addParts[0], addParts[1], int.Parse(addParts[2]), int.Parse(addParts[3])));
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
