
using System;

class Program
{
    public string CleanseAndInvert(string input)
    {

        if (string.IsNullOrEmpty(input) || input.Length < 6)
        {
            return "";
        }


        foreach (char c in input)
        {
            if (!char.IsLetter(c))
            {
                return "";
            }
        }


        input = input.ToLower();


        string filtered = "";
        foreach (char c in input)
        {
            if (((int)c) % 2 != 0)
            {
                filtered += c;
            }
        }


        char[] reversed = filtered.ToCharArray();
        Array.Reverse(reversed);

                for (int i = 0; i < reversed.Length; i++)
        {
            if (i % 2 == 0)
            {
                reversed[i] = char.ToUpper(reversed[i]);
            }
        }

        return new string(reversed);
    }

    static void Main(string[] args)
    {
        Console.WriteLine("Enter the word");
        string input = Console.ReadLine();

        Program obj = new Program();
        string result = obj.CleanseAndInvert(input);

        if (result == "")
        {
            Console.WriteLine("Invalid Input");
        }
        else
        {
            Console.WriteLine("The generated key is - " + result);
        }
    }
}
