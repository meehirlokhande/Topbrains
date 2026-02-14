using System;
using System.Data.Common;

namespace BookStoreApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            // TODO:
            // 1. Read initial input
            // Format: BookID Title Price Stock
            Console.WriteLine("Enter Book Details: ");
            string input = Console.ReadLine();
            string[] str = input.Split(' ');
            string BookID = str[0];
            string BookTitle = str[1];
            int BookPrice = int.Parse(str[2]);
            int BookStock = int.Parse(str[3]);




            Book book = new Book()
            {
                Id = BookID,
                Title = BookTitle,
                Price = BookPrice,
                Stock = BookStock
            };

            BookUtility utility = new BookUtility(book);

            while (true)
            {
                // TODO:
                // Display menu:
                // 1 -> Display book details
                // 2 -> Update book price
                // 3 -> Update book stock
                // 4 -> Exit
                Console.WriteLine("1 -> Display book details");
                Console.WriteLine("2 -> Update price (Next line contains new price)");
                Console.WriteLine("3-> Update stock (Next line contains new stock)");
                Console.WriteLine("4 -> Exit");


                int choice = 0; // TODO: Read user choice
                choice = int.Parse(Console.ReadLine());


                switch (choice)
                {
                    case 1:
                        utility.GetBookDetails();
                        break;

                    case 2:
                        // TODO:
                        // Read new price
                        int newPrice = int.Parse(Console.ReadLine());
                        utility.UpdateBookPrice(newPrice);
                        // Call UpdateBookPrice()
                        break;

                    case 3:
                        // TODO:
                        // Read new stock
                        int newStock = int.Parse(Console.ReadLine());
                        utility.UpdateBookStock(newStock);
                        // Call UpdateBookStock()
                        break;

                    case 4:
                        Console.WriteLine("Thank You");
                        return;

                    default:
                        // TODO: Handle invalid choice
                        break;
                }
            }
        }
    }
}
