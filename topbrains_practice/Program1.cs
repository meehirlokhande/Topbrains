using System;
using System.Collections.Generic;

public class Program1
{
    public static Stack<Order> OrderStack = new Stack<Order>();

    public static void Main(string[] args)
    {
        int id = Convert.ToInt32(Console.ReadLine());
        string name = Console.ReadLine();
        string item = Console.ReadLine();

        Order order = new Order();

        order.AddOrderDetails(id, name, item);

        Console.WriteLine(order.GetOrderDetails());
        order.RemoveOrderDetails();

        ///Task -2
        int[] arr = new int[]{0,1,0,3,12};
        int[] arr1 = new int[]{4, 0, 5, 0, 0, 7, 8};
        int[] arr2 = new int[]{0, 0, 0};
        int[] arr3 = new int[]{};

        int n;
        n = int.Parse(Console.ReadLine());
        int[] arr4 = new int[n];
        for(int i=0;i<n;i++){
            arr4[i] = int.Parse(Console.ReadLine());
        }

        order.SwapInPlace(arr);
        foreach(int num in arr){
            Console.Write(num + " ");
        }
        order.SwapInPlace(arr1);
        foreach(int num in arr1){
            Console.Write(num + " ");
        }
        order.SwapInPlace(arr2);
        foreach(int num in arr2){
            Console.Write(num + " ");
        }
        order.SwapInPlace(arr3);
        foreach(int num in arr3){
            Console.Write(num + " ");
        }
        order.SwapInPlace(arr4);
    }
}

public class Order
{
    public int OrderId { get; set; }
    public string CustomerName { get; set; }
    public string Item { get; set; }

    public Stack<Order> AddOrderDetails(int orderId, string customerName, string item)
    {
        Order newOrder = new Order();
        newOrder.OrderId = orderId;
        newOrder.CustomerName = customerName;
        newOrder.Item = item;

        Program1.OrderStack.Push(newOrder);
        return Program1.OrderStack;
    }

    public string GetOrderDetails()
    {
        Order top = Program1.OrderStack.Peek();
        return top.OrderId + " " + top.CustomerName + " " + top.Item;
    }

    public Stack<Order> RemoveOrderDetails()
    {
        Program1.OrderStack.Pop();
        return Program1.OrderStack;
    }

    public SwapInPlace(int[] arr){
        int insertPosition = 0;

        for(int i=0;i<arr.Length;i++){
            if(arr[i]!=0){
                arr[insertPosition] = arr[i];
                insertPosition++;
            }
        }

        while(insertPosition<arr.Length){
            arr[insertPosition] = 0;
            insertPosition++;
        }
    }
}
