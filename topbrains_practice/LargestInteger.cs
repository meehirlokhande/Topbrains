using System;

class LargestInteger{
    public static void Main(string[] args){
        int a = int.Parse(Console.ReadLine());
        int b = int.Parse(Console.ReadLine());
        int c = int.Parse(Console.ReadLine());
        int max = Math.Max(a,Math.Max(b,c));
        Console.WriteLine(max);
    }
}