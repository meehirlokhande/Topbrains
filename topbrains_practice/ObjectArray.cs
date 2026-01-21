using System;
class ObjectArray{
    public static void Main(string[] args){
        object[] arr = new object[]{1,"hello",3.14,true,null};
        int sum = 0;
        foreach(var obj in arr){
            if(obj is int x ){
                    sum += x;
            }
        }

        Console.WriteLine(sum);
    }
}