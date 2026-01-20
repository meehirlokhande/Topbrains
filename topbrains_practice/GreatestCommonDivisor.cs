using System;
class GreatestCommonDivisor{
    public static int GCD(int a,int b){
        if(a==b){
            return a;
        }
        if(a>b){
            return GCD(a-b,b);
        }
        return GCD(a,b-a);
    }
    public static void Main(string[] args){
        int a = int.Parse(Console.ReadLine());
        int b = int.Parse(Console.ReadLine());
        int gcd = GCD(a,b);
        Console.WriteLine(gcd);
    }
}