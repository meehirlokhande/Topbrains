using System;
using System.Globalization;
class InventoryNameCleanup{
    public static void Main(string[] args){
        string str = Console.ReadLine();
        str = str.Trim();
        for(int i=0;i<str.Length;i++){
            for(int j=i+1;j<str.Length;j++){
                if(str[i]==str[j]){
                    str = str.Remove(j,1);
                }
            }
        }
        //Convert to Title Case
        str = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(str);
        Console.WriteLine(str);
    }
}