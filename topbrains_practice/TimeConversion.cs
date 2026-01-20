using System;
class TimeConversion{
    public static void Main(string[] args){
        int seconds = int.Parse(Console.ReadLine());
        double minutes = seconds/60.0;
        double min = Math.Round(minutes,2);
        if(min%1==0){
            string minString = min.ToString();
            Console.WriteLine($"{minString}:00");
        }else{
            string minString = min.ToString();
            Console.WriteLine($"{minString} minutes");
        }
    }
}