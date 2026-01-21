using System;
class BankTransactionSystem{
        public static void Main(string[] args){
            int initialBalance = int.Parse(Console.ReadLine());
            int n = int.Parse(Console.ReadLine());
        int[] transactions = new int[n];
        for(int i=0;i<transactions.Length;i++){
            transactions[i] = int.Parse(Console.ReadLine());
        }

        for(int i=0;i<transactions.Length;i++){
            if(transactions[i] > 0){
                initialBalance += transactions[i];
            }else if(transactions[i] < 0){
                initialBalance -= transactions[i];
            }else{
                continue;
            }
        }
        Console.WriteLine(initialBalance);
        }

}