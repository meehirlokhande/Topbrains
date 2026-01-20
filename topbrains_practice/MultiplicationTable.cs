using System;
class MultiplicationTable{
    public static void Main(string[] args){
        int n = int.Parse(Console.ReadLine());
        int upto = int.Parse(Console.ReadLine());
        int[] table = new int[upto+1];
        for(int i=1;i<=upto;i++){
            table[i] = n*i;
        }
        for(int i=1;i<=upto;i++){
            Console.Write($"{table[i]}  ");
        }
    }
}