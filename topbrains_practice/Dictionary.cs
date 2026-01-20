using System;
using System.Collections.Generic;
class Dictionary{
    public static void Main(string[] args){
        Dictionary<int,int>dict = new Dictionary<int,int>();
        dict.Add(1,20000);
        dict.Add(2,60000);
        dict.Add(3,70000);
        dict.Add(4,90000);
        dict.Add(5,80000);

        List<int>Ids = new List<int>();
        Ids.Add(1);
        Ids.Add(2);
        Ids.Add(3);
        Ids.Add(4);
        Ids.Add(5);

        int totalSalary = 0;
        foreach(int id in Ids){
            totalSalary += dict[id];
        }
        Console.WriteLine(totalSalary);
    }
}