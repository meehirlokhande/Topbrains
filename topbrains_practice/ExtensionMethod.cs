using System;
class ExtensionMethod
{
    public static void Main(string[] args)
    {
        string[] str = { "1:Meehir", "2:Shivam", "3:Tuna", "4:Alex" };

        string[] result = GetDistinctNames(str);
    }

    static string[] GetDistinctNames(string[] str)
    {
        if (str == null || str.Length == 0)
        {
            return Array.Empty<string>();
        }

        HashSet<string> seenIds = new HashSet<string>();
        List<string> names = new List<string>();

        foreach (string item in str)
        {
            string[] parts = item.Split(':');
            string id = parts[0];
            string name = parts[1];

            if (seenIds.Add(id))
            {
                names.Add(name);
            }
        }

        return names.ToArray();
    }
}