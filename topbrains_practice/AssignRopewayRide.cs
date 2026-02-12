using System;

public class Solution
{
    public static string CheckEligibility(int age, int weight)
    {
        if(age >= 18 && weight < 90){
            return "You are allowed to Take Ropeway Ride";
        }else{
            return"You are not allowed to Take Ropeway Ride";
        }
    }

   

   
}