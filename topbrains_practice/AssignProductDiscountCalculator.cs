using System;

class AssignProductDiscountCalculator
{
    static void Main()
    {
        // Sample Inputs
        double orderAmountA = 6000;
        double orderAmountB = 12000;

        // Calculate and display discounts
        CalculateAndPrintDiscount(orderAmountA, orderAmountB);
    }

    // Method to calculate discount for Product A
    static double GetDiscountForA(double amount)
    {
        //Write Your Logic Here
        if(amount > 5000){
            double dis = 6000*0.05;
            return dis;
        }else{
            return 0.0;
        }
        
    }

    // Method to calculate discount for Product B
    static double GetDiscountForB(double amount)
    {
        //Write Your Logic Here
        if(amount > 10000){
            double dis = 12000*0.07;
            return dis;
        }else{
            return 0.0;
        }
        
    }

    // Method to display results
    static void CalculateAndPrintDiscount(double amountA, double amountB)
    {
        //Write Your Logic Here
        Console.WriteLine($"Product A Order Amount : Rs. {amountA}");
        Console.WriteLine($"Product A Discount     : Rs. {GetDiscountForA(amountA):F2}");
        Console.WriteLine($"Product B Order Amount : Rs. {amountB}");
        Console.WriteLine($"Product B Discount     : Rs. {GetDiscountForB(amountB):F2}");

    }
}
