using System.Collections.Generic;
using System.Globalization;
using System.Text.Json;

namespace CampusHire;

class Program
{
    static List<Applicant> applicants = new List<Applicant>();
    static string dataFile = "applicants.json";

    static void Main()
    {
        LoadData();

        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("1. Add Applicant");
            Console.WriteLine("2. Display All");
            Console.WriteLine("3. Search by ID");
            Console.WriteLine("4. Update Applicant");
            Console.WriteLine("5. Delete Applicant");
            Console.WriteLine("6. Exit");
            Console.Write("Enter choice: ");
            string? ch = Console.ReadLine();

            if (ch == "6")
            {
                SaveData();
                return;
            }

            if (ch == "1") AddNewApplicant();
            else if (ch == "2") ShowAll();
            else if (ch == "3") FindById();
            else if (ch == "4") EditApplicant();
            else if (ch == "5") DeleteApplicant();
            else Console.WriteLine("Invalid choice");
        }
    }

    static void AddNewApplicant()
    {
        string id = Read("Applicant ID: ");
        string err = ValidateId(id);
        if (err != "")
        {
            Console.WriteLine(err);
            return;
        }
        if (applicants.Exists(a => a.ApplicantId.Equals(id, StringComparison.OrdinalIgnoreCase)))
        {
            Console.WriteLine("Applicant ID already exists");
            return;
        }

        string name = Read("Applicant Name: ");
        err = ValidateName(name);
        if (err != "") { Console.WriteLine(err); return; }

        string curLoc = Read("Current Location (Mumbai/Pune/Chennai): ");
        err = ValidateCurrentLoc(curLoc);
        if (err != "") { Console.WriteLine(err); return; }

        string prefLoc = Read("Preferred Location (Mumbai/Pune/Chennai/Delhi/Kolkata/Bangalore): ");
        err = ValidatePreferredLoc(prefLoc);
        if (err != "") { Console.WriteLine(err); return; }

        string comp = Read("Core Competency (.NET/JAVA/ORACLE/Testing): ");
        err = ValidateCompetency(comp);
        if (err != "") { Console.WriteLine(err); return; }

        string yrStr = Read("Passing Year: ");
        err = ValidateYear(yrStr, out int year);
        if (err != "") { Console.WriteLine(err); return; }

        applicants.Add(new Applicant
        {
            ApplicantId = id,
            ApplicantName = name,
            CurrentLocation = curLoc,
            PreferredLocation = prefLoc,
            CoreCompetency = comp,
            PassingYear = year
        });
        SaveData();
        Console.WriteLine("Applicant added.");
    }

    static void ShowAll()
    {
        if (applicants.Count == 0)
        {
            Console.WriteLine("No records found");
            return;
        }
        foreach (var a in applicants)
            Console.WriteLine($"{a.ApplicantId} {a.ApplicantName} {a.CurrentLocation} {a.PreferredLocation} {a.CoreCompetency} {a.PassingYear}");
    }

    static void FindById()
    {
        string id = Read("Enter Applicant ID: ");
        var a = applicants.Find(x => x.ApplicantId.Equals(id, StringComparison.OrdinalIgnoreCase));
        if (a == null)
        {
            Console.WriteLine("Applicant not found");
            return;
        }
        Console.WriteLine($"{a.ApplicantId} {a.ApplicantName} {a.CurrentLocation} {a.PreferredLocation} {a.CoreCompetency} {a.PassingYear}");
    }

    static void EditApplicant()
    {
        string id = Read("Enter Applicant ID to update: ");
        var a = applicants.Find(x => x.ApplicantId.Equals(id, StringComparison.OrdinalIgnoreCase));
        if (a == null)
        {
            Console.WriteLine("Applicant not found");
            return;
        }

        string name = Read($"Name [{a.ApplicantName}]: ");
        if (!string.IsNullOrWhiteSpace(name) && ValidateName(name) == "")
            a.ApplicantName = name;

        string curLoc = Read($"Current Location [{a.CurrentLocation}]: ");
        if (curLoc == "Mumbai" || curLoc == "Pune" || curLoc == "Chennai")
            a.CurrentLocation = curLoc;

        string prefLoc = Read($"Preferred Location [{a.PreferredLocation}]: ");
        if (prefLoc == "Mumbai" || prefLoc == "Pune" || prefLoc == "Chennai" || prefLoc == "Delhi" || prefLoc == "Kolkata" || prefLoc == "Bangalore")
            a.PreferredLocation = prefLoc;

        string comp = Read($"Core Competency [{a.CoreCompetency}]: ");
        if (comp == ".NET" || comp == "JAVA" || comp == "ORACLE" || comp == "Testing")
            a.CoreCompetency = comp;

        string yrStr = Read($"Passing Year [{a.PassingYear}]: ");
        if (int.TryParse(yrStr, NumberStyles.Integer, CultureInfo.InvariantCulture, out int y) && y <= DateTime.Now.Year)
            a.PassingYear = y;

        SaveData();
        Console.WriteLine("Applicant updated.");
    }

    static void DeleteApplicant()
    {
        string id = Read("Enter Applicant ID to delete: ");
        var a = applicants.Find(x => x.ApplicantId.Equals(id, StringComparison.OrdinalIgnoreCase));
        if (a == null)
        {
            Console.WriteLine("Applicant not found");
            return;
        }
        applicants.Remove(a);
        SaveData();
        Console.WriteLine("Applicant deleted.");
    }

    static string Read(string prompt)
    {
        Console.Write(prompt);
        return (Console.ReadLine() ?? "").Trim();
    }

    static string ValidateId(string id)
    {
        if (string.IsNullOrWhiteSpace(id)) return "Applicant ID is required.";
        if (id.Length != 8) return "Applicant ID must be 8 characters.";
        if (!id.StartsWith("CH", StringComparison.OrdinalIgnoreCase)) return "Applicant ID must start with CH.";
        return "";
    }

    static string ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) return "Name is required.";
        if (name.Length < 4) return "Name must be at least 4 characters.";
        if (name.Length > 15) return "Name must be at most 15 characters.";
        return "";
    }

    static string ValidateCurrentLoc(string loc)
    {
        if (loc != "Mumbai" && loc != "Pune" && loc != "Chennai")
            return "Current location must be Mumbai, Pune or Chennai.";
        return "";
    }

    static string ValidatePreferredLoc(string loc)
    {
        if (loc != "Mumbai" && loc != "Pune" && loc != "Chennai" && loc != "Delhi" && loc != "Kolkata" && loc != "Bangalore")
            return "Preferred location must be one of: Mumbai, Pune, Chennai, Delhi, Kolkata, Bangalore.";
        return "";
    }

    static string ValidateCompetency(string c)
    {
        if (c != ".NET" && c != "JAVA" && c != "ORACLE" && c != "Testing")
            return "Core competency must be .NET, JAVA, ORACLE or Testing.";
        return "";
    }

    static string ValidateYear(string yrStr, out int year)
    {
        year = 0;
        if (string.IsNullOrWhiteSpace(yrStr)) return "Passing year is required.";
        if (!int.TryParse(yrStr, NumberStyles.Integer, CultureInfo.InvariantCulture, out year)) return "Invalid year.";
        if (year > DateTime.Now.Year) return "Passing year cannot be in the future.";
        return "";
    }

    static void SaveData()
    {
        File.WriteAllText(dataFile, JsonSerializer.Serialize(applicants));
    }

    static void LoadData()
    {
        if (!File.Exists(dataFile)) return;
        try
        {
            var list = JsonSerializer.Deserialize<List<Applicant>>(File.ReadAllText(dataFile));
            if (list != null) applicants = list;
        }
        catch { }
    }
}
