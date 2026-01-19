using System;
class Programming{
    public static int S(int x){
        int sum = 0;
        while(x>0){
            int digit = x%10;
            sum += digit;
            x = x/10;
        }
        return sum;
    }
    public static void Main(string[] args){
            int m = int.Parse(Console.ReadLine());
            int n = int.Parse(Console.ReadLine());
            int totalLuckyNumbers = 0;
            for(int i=m;i<=n;i++){
                if(S(i*i)==S(i)*S(i)){
                    totalLuckyNumbers++;
                }
            }
            Console.WriteLine(totalLuckyNumbers);
    }
}