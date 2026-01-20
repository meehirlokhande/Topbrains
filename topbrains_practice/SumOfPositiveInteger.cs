using System;
class SumOfPositiveInteger{
    public static void Main(string[] args){
        int[] arr = new int[]{1,2,3,4,-1,-2,4,0};
        int sum=0;
        for(int i=0;i<arr.Length;i++){
            if(arr[i] == 0){
                break;
            }else if(arr[i] < 0){
                continue;
            }else{
                sum += arr[i];
            }
        }

        Console.WriteLine(sum);
    }
}