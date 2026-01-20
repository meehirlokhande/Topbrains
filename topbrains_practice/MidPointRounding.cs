using System;

class MidPointRounding{
    public static void Main(string[] args){
        double radius = double.Parse(Console.ReadLine());

        double area = Math.PI*radius*radius;
        area = Math.Round(area,2);

        Console.WriteLine(area);
    }
}