using System;



class Swapping{

    public static void  SwapWithRef(ref int a,ref int b){
    int temp;
    temp = a;
    a = b;
    b = temp;
}

public static void AssignWithOut(out int a,out int b){
    a = 100;
    b = 200;

}




    public static void Main(string[] args){
        int a = 10;
        int b = 20;

         Console.WriteLine("Before: a={0}, b={1}", a, b);

        SwapWithRef(ref a, ref b);
        Console.WriteLine("After ref swap: a={0}, b={1}", a, b);

        AssignWithOut(out a, out b);
        Console.WriteLine("After out assign: a={0}, b={1}", a, b);

    }
}