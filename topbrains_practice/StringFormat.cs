using System;
using System.Collections.Generic;
using System.Text.Json;

record SStudent(string Name, int Score);
class StringFormat
{
    public static void Main(string[] args)
    {
        string[] items = { "Alice:85", "Bob:72", "Charlie:90", "Diana:65", "Eve:90" };
        int minScore = 75;

        string json = FormatStudents(items, minScore);
        Console.WriteLine(json);
    }

    static string FormatStudents(string[] items, int minScore)
    {
        if (items == null || items.Length == 0)
        {
            return "[]";
        }

        List<SStudent> students = new List<SStudent>();

        foreach (string item in items)
        {
            string[] parts = item.Split(':');
            string name = parts[0];
            int score = int.Parse(parts[1]);

            if (score >= minScore)
            {
                students.Add(new SStudent(name, score));
            }
        }

        students.Sort((a, b) =>
        {
            int cmp = b.Score.CompareTo(a.Score);
            if (cmp != 0) return cmp;
            return string.Compare(a.Name, b.Name, StringComparison.Ordinal);
        });

        return JsonSerializer.Serialize(students);

    }
}