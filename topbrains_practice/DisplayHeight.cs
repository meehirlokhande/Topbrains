using System;
class DisplayHeight{
    public static void Main(string[] args){
        int heightCm = int.Parse(Console.ReadLine());

        if(heightCm<150){
            Console.WriteLine("Short");
        }
        else if(150 <= heightCm && heightCm < 180){
            Console.WriteLine("Average");
        }else{
            Console.WriteLine("Tall");
        }
    }
}