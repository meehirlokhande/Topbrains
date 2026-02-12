using System;

class InsurancePremium
{
    static void Main()
    {
        Console.WriteLine("--- Insurance Premium & Claim System ---");

        Console.Write("Enter Customer Name: ");
        string name ="Rajesh Patel";

        Console.Write("Enter Age: ");
        int age = 32;

        Console.Write("Is Policy Active? (yes/no): ");
        string policy = "yes";

        Console.Write("Enter number of months paid: ");
        int months = 12;

        Console.Write("Enter Coverage Amount: ");
        double coverage = 100000;

        Console.Write("Enter Claim Amount: ");
        double claim = 50000;

        // Premium Calculation
        double premium = age switch
        {
            >= 18 and <= 30 => 1200,
            >= 31 and <= 50 => 1800,
            >= 51 and <= 60 => 2500,
            _ => 3000
        };



        // Claim Eligibility
        string claimStatus =
            (policy == "yes" && months >= 6 && claim < coverage)
            ? "APPROVED"
            : "REJECTED";

        // Report
        Console.WriteLine("\n--- Customer Insurance Report ---");
        Console.WriteLine($"Name: {name}");
        Console.WriteLine($"Age: {age}");
        Console.WriteLine($"Monthly Premium: ₹{premium}");
        Console.WriteLine($"Coverage: ₹{coverage}");
        Console.WriteLine($"Claim Amount: ₹{claim}");
        Console.WriteLine($"Claim Status: {claimStatus}");   
    }
}