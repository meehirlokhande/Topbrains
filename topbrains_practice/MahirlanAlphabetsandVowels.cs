using System;
class MahirlanAlphabetsandVowels{

    public static void Main(string[] args){

        string firstWord = Console.ReadLine();
        string secondWord = Console.ReadLine();

        //Task - 1 Remove common Consonants;
        string vowels = "aeiouAEIOU";
        for(int i=0;i<firstWord.Length;i++){
            if(secondWord.Contains(firstWord[i]) && !vowels.Contains(firstWord[i])){
                firstWord = firstWord.Remove(i,1);
            }

        }
        //Task - 2 Remove duplicate alphabets;
       for(int i=0;i<firstWord.Length;i++){
        for(int j=i+1;j<firstWord.Length;j++){
            if(firstWord[i]==firstWord[j]){
                firstWord = firstWord.Remove(j,1);
            }
        }
       }

       Console.WriteLine("Final Processed Word: {0}", firstWord);

    }
}