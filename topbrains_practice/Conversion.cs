using System;
class Conversion{
    public static void Main(string[] args){
        int n = int.Parse(Console.ReadLine());
        double centi = n*30.48;
        centi = Math.Round(centi,2);
        Console.WriteLine(centi);
    }
}