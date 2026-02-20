using System;


using System.Collections.Generic;


using System.Linq;












public interface IPatient


{


    int PatientId { get; }


    string Name { get; }


    DateTime DateOfBirth { get; }


    BloodType BloodType { get; }


}





public enum BloodType { A, B, AB, O }


public enum Condition { Stable, Critical, Recovering }





public class PediatricPatient : IPatient


{


    public int PatientId { get; set; }

    public string Name { get; set; } = string.Empty;


    public DateTime DateOfBirth { get; set; }


    public BloodType BloodType { get; set; }





    public string GuardianName { get; set; } = string.Empty;


    public double Weight { get; set; } // kg


}





public class GeriatricPatient : IPatient


{


    public int PatientId { get; set; }


    public string Name { get; set; } = string.Empty;


    public DateTime DateOfBirth { get; set; }


    public BloodType BloodType { get; set; }





    public List<string> ChronicConditions { get; } = new();


    public int MobilityScore { get; set; } // 1-10


}

public class PriorityQueue<T> where T : IPatient


{


    private readonly SortedDictionary<int, Queue<T>> _queues = new();


    private const int MinPriority = 1;


    private const int MaxPriority = 5;





    public void Enqueue(T patient, int priority)


    {


        if (priority < MinPriority || priority > MaxPriority)


            throw new ArgumentOutOfRangeException(nameof(priority));





        if (!_queues.ContainsKey(priority))


            _queues[priority] = new Queue<T>();





        _queues[priority].Enqueue(patient);


    }





    public T Dequeue()


    {

        foreach (var queue in _queues)


        {


            if (queue.Value.Count > 0)


                return queue.Value.Dequeue();


        }





        throw new InvalidOperationException("Queue is empty.");


    }





    public T Peek()


    {


        foreach (var queue in _queues)


        {


            if (queue.Value.Count > 0)


                return queue.Value.Peek();


        }





        throw new InvalidOperationException("Queue is empty.");


    }

    public int GetCountByPriority(int priority)


    {


        return _queues.ContainsKey(priority) ? _queues[priority].Count : 0;


    }


}





public class MedicalRecord<T> where T : IPatient


{


    private readonly T _patient;


    private readonly List<(DateTime date, string diagnosis)> _diagnoses = new();


    private readonly Dictionary<DateTime, string> _treatments = new();





    public MedicalRecord(T patient)


    {


        _patient = patient;


    }





    public void AddDiagnosis(string diagnosis, DateTime date)


    {


        _diagnoses.Add((date, diagnosis));

    }





    public void AddTreatment(string treatment, DateTime date)


    {


        _treatments[date] = treatment;


    }





    public IEnumerable<KeyValuePair<DateTime, string>> GetTreatmentHistory()


    {


        return _treatments.OrderBy(x => x.Key);


    }


}












public class MedicationSystem<T> where T : IPatient


{


    private readonly Dictionary<T, List<(string medication, DateTime time)>> _medications = new();





    public void PrescribeMedication(

    T patient,


    string medication,


    Func<T, bool> dosageValidator)


    {


        if (!dosageValidator(patient))


            throw new InvalidOperationException("Dosage validation failed.");





        if (!_medications.ContainsKey(patient))


            _medications[patient] = new List<(string, DateTime)>();





        if (CheckInteractions(patient, medication))


            Console.WriteLine($"⚠ Warning: Possible interaction detected for {medication}");





        _medications[patient].Add((medication, DateTime.Now));


        Console.WriteLine($"✔ {medication} prescribed to {patient.Name}");


    }





    public bool CheckInteractions(T patient, string newMedication)


    {


        if (!_medications.ContainsKey(patient))

            return false;





        var existing = _medications[patient].Select(m => m.medication);





        // Simple demo interaction rule


        if (existing.Contains("Aspirin") && newMedication == "Warfarin")


            return true;





        return false;


    }


}





class Program


{


    static void Main()


    {


        Console.WriteLine("===== HOSPITAL WORKFLOW SIMULATION =====\n");





        // Create Patients


        var p1 = new PediatricPatient

        {


            PatientId = 1,


            Name = "Rahul",


            DateOfBirth = DateTime.Now.AddYears(-5),


            BloodType = BloodType.A,


            GuardianName = "Mr. Sharma",


            Weight = 18


        };


        var p2 = new PediatricPatient


        {


            PatientId = 2,


            Name = "Anaya",


            DateOfBirth = DateTime.Now.AddYears(-8),


            BloodType = BloodType.O,


            GuardianName = "Mrs. Verma",


            Weight = 12


        };





        var g1 = new GeriatricPatient


        {

            PatientId = 3,


            Name = "Mr. Iyer",


            DateOfBirth = DateTime.Now.AddYears(-70),


            BloodType = BloodType.B,


            MobilityScore = 4


        };





        var g2 = new GeriatricPatient


        {


            PatientId = 4,


            Name = "Mrs. Kapoor",


            DateOfBirth = DateTime.Now.AddYears(-75),


            BloodType = BloodType.AB,


            MobilityScore = 2


        };





        // Priority Queue


        var queue = new PriorityQueue<IPatient>();


        queue.Enqueue(p1, 2);


        queue.Enqueue(g1, 1);

        queue.Enqueue(p2, 3);


        queue.Enqueue(g2, 2);


        Console.WriteLine("Processing Patients by Priority:");


        while (true)


        {


            try


            {


                var patient = queue.Dequeue();


                Console.WriteLine($"→ Processing: {patient.Name}");


            }


            catch


            {


                break;


            }


        }


        // Medical Record


        var record = new MedicalRecord<PediatricPatient>(p1);


        record.AddDiagnosis("Flu", DateTime.Now.AddDays(-2));


        record.AddTreatment("Paracetamol", DateTime.Now.AddDays(-1));

        Console.WriteLine("\nTreatment History:");


        foreach (var t in record.GetTreatmentHistory())


        {


            Console.WriteLine($"{t.Key.ToShortDateString()} : {t.Value}");


        }





        // Medication System


        var pediatricMedSystem = new MedicationSystem<PediatricPatient>();


        var geriatricMedSystem = new MedicationSystem<GeriatricPatient>();





        Console.WriteLine("\nMedication Prescriptions:");





        // Pediatric (weight-based validation)


        pediatricMedSystem.PrescribeMedication(


        p1,


        "Cough Syrup",


        patient => patient.Weight > 15


        );





        try

        {


            pediatricMedSystem.PrescribeMedication(


            p2,


            "Strong Antibiotic",


            patient => patient.Weight > 15


            );


        }


        catch (Exception ex)


        {


            Console.WriteLine($"✖ {p2.Name}: {ex.Message}");


        }





        // Geriatric (mobility-based validation)


        geriatricMedSystem.PrescribeMedication(


        g1,


        "Aspirin",


        patient => patient.MobilityScore >= 3


        );





        geriatricMedSystem.PrescribeMedication(

        g1,


        "Warfarin",


        patient => patient.MobilityScore >= 3


        );





        try


        {


            geriatricMedSystem.PrescribeMedication(


            g2,


            "Painkiller",


            patient => patient.MobilityScore >= 3


            );


        }


        catch (Exception ex)


        {


            Console.WriteLine($"✖ {g2.Name}: {ex.Message}");


        }





        Console.WriteLine("\n===== END OF SIMULATION =====");


    }

}
