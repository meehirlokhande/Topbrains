using System;
class Parsing{
    public static void Main(string[] args){
        string[] arr = new string[]{"1","2","3","4","5"};
        int sum=0;
        for(int i=0;i<arr.Length;i++){
            if(int.TryParse(arr[i],out int num)){
                sum += num;
            }
        }
        Console.WriteLine(sum);
    }
}
