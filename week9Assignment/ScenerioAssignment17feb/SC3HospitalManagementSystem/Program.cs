#nullable disable
// Exceptions
class DoctorNotAvailableException : Exception { public DoctorNotAvailableException(string m) : base(m) { } }
class InvalidAppointmentException : Exception { public InvalidAppointmentException(string m) : base(m) { } }
class PatientNotFoundException : Exception { public PatientNotFoundException(string m) : base(m) { } }
class DuplicateMedicalRecordException : Exception { public DuplicateMedicalRecordException(string m) : base(m) { } }

class Person
{
    public int Id { get; set; }
    public string Name { get; set; }
}

class Doctor : Person
{
    public string Specialization { get; set; }
    public double ConsultationFee { get; set; }
}

class Patient : Person
{
    public string Disease { get; set; }
}

class MedicalRecord
{
    int _recordId;
    string _details;
    public int RecordId => _recordId;
    public string Details { get => _details; set => _details = value; }
    public int PatientId { get; set; }
    public DateTime Date { get; set; }

    public MedicalRecord(int id, int pid, string details, DateTime date)
    {
        _recordId = id;
        PatientId = pid;
        _details = details;
        Date = date;
    }
}

class Appointment
{
    public int Id { get; set; }
    public Doctor Doctor { get; set; }
    public Patient Patient { get; set; }
    public DateTime SlotStart { get; set; }
    public DateTime SlotEnd { get; set; }
    public bool IsCompleted { get; set; }
}

interface IBillable
{
    double CalculateBill();
}

class AppointmentBill : IBillable
{
    Appointment _apt;
    public AppointmentBill(Appointment apt) { _apt = apt; }
    public double CalculateBill() => _apt.Doctor.ConsultationFee;
}

class Hospital
{
    public List<Doctor> Doctors { get; } = new List<Doctor>();
    public List<Patient> Patients { get; } = new List<Patient>();
    public List<Appointment> Appointments { get; } = new List<Appointment>();
    public Dictionary<int, MedicalRecord> MedicalRecords { get; } = new Dictionary<int, MedicalRecord>();
    int _recordIdCounter = 1;

    public Appointment BookAppointment(int doctorId, int patientId, DateTime slotStart)
    {
        var doc = Doctors.FirstOrDefault(d => d.Id == doctorId);
        if (doc == null) throw new DoctorNotAvailableException("Doctor not found");
        var pat = Patients.FirstOrDefault(p => p.Id == patientId);
        if (pat == null) throw new PatientNotFoundException("Patient not found");
        var slotEnd = slotStart.AddMinutes(30);
        bool overlap = Appointments.Any(a => a.Doctor.Id == doctorId && a.SlotStart < slotEnd && a.SlotEnd > slotStart);
        if (overlap) throw new InvalidAppointmentException("Slot overlaps with existing appointment");
        var apt = new Appointment { Id = Appointments.Count + 1, Doctor = doc, Patient = pat, SlotStart = slotStart, SlotEnd = slotEnd };
        Appointments.Add(apt);
        return apt;
    }

    public void AddMedicalRecord(int patientId, string details)
    {
        if (MedicalRecords.Values.Any(r => r.PatientId == patientId && r.Date.Date == DateTime.Today))
            throw new DuplicateMedicalRecordException("Record already exists for today");
        var r = new MedicalRecord(_recordIdCounter++, patientId, details, DateTime.Now);
        MedicalRecords[r.RecordId] = r;
    }

    public void ExportAppointmentReport()
    {
        Console.WriteLine("=== Appointment Report ===");
        foreach (var a in Appointments.OrderBy(a => a.SlotStart))
        {
            Console.WriteLine($"Appt {a.Id} | Dr.{a.Doctor.Name} | Patient {a.Patient.Name} | {a.SlotStart:yyyy-MM-dd HH:mm} - {a.SlotEnd:HH:mm}");
        }
    }
}

class Program
{
    static Hospital hospital = new Hospital();

    static void Main(string[] args)
    {
        hospital.Doctors.Add(new Doctor { Id = 1, Name = "Dr. Sharma", Specialization = "Cardio", ConsultationFee = 800 });
        hospital.Doctors.Add(new Doctor { Id = 2, Name = "Dr. Patel", Specialization = "General", ConsultationFee = 500 });
        hospital.Patients.Add(new Patient { Id = 1, Name = "Ramesh", Disease = "Fever" });
        hospital.Patients.Add(new Patient { Id = 2, Name = "Sita", Disease = "BP" });
        hospital.Patients.Add(new Patient { Id = 3, Name = "Vijay", Disease = "Fever" });

        var apt1 = hospital.BookAppointment(1, 1, DateTime.Now.AddDays(-5));
        apt1.IsCompleted = true;
        hospital.BookAppointment(1, 2, DateTime.Now.AddDays(-2));
        for (int i = 0; i < 12; i++) hospital.BookAppointment(1, 1, DateTime.Now.AddDays(-20 + i));
        hospital.AddMedicalRecord(1, "Fever, prescribed rest");

        while (true)
        {
            Console.WriteLine("\n--- Hospital ---");
            Console.WriteLine("1. Book appointment  2. Add medical record  3. Export report");
            Console.WriteLine("4. Doctors >10 appts  5. Patients last 30 days  6. Group by doctor  7. Top 3 earning doctors  8. Patients by disease  9. Total revenue  0. Exit");
            string c = Console.ReadLine();
            if (c == "0") break;
            try
            {
                if (c == "1") BookApt();
                else if (c == "2") { Console.Write("PatientId and details: "); var s = Console.ReadLine().Split(new[] { ' ' }, 2); hospital.AddMedicalRecord(int.Parse(s[0]), s.Length > 1 ? s[1] : ""); }
                else if (c == "3") hospital.ExportAppointmentReport();
                else if (c == "4") LinqDoctorsMore10();
                else if (c == "5") LinqPatientsLast30();
                else if (c == "6") LinqGroupByDoctor();
                else if (c == "7") LinqTop3Earning();
                else if (c == "8") LinqPatientsByDisease();
                else if (c == "9") LinqTotalRevenue();
            }
            catch (Exception ex) { Console.WriteLine("Error: " + ex.Message); }
        }
    }

    static void BookApt()
    {
        Console.Write("DoctorId PatientId (e.g. 1 2): ");
        var s = Console.ReadLine().Split();
        Console.Write("Date (yyyy-mm-dd) and hour (e.g. 2025-03-01 10): ");
        var d = Console.ReadLine().Split();
        var dt = DateTime.Parse(d[0]).AddHours(int.Parse(d[1]));
        hospital.BookAppointment(int.Parse(s[0]), int.Parse(s[1]), dt);
        Console.WriteLine("Booked.");
    }

    static void LinqDoctorsMore10()
    {
        var list = hospital.Appointments.GroupBy(a => a.Doctor.Id).Where(g => g.Count() > 10).Select(g => hospital.Doctors.First(d => d.Id == g.Key));
        foreach (var d in list) Console.WriteLine(d.Name);
    }

    static void LinqPatientsLast30()
    {
        var cutoff = DateTime.Now.AddDays(-30);
        var list = hospital.Appointments.Where(a => a.SlotStart >= cutoff).Select(a => a.Patient).Distinct();
        foreach (var p in list) Console.WriteLine(p.Name);
    }

    static void LinqGroupByDoctor()
    {
        foreach (var g in hospital.Appointments.GroupBy(a => a.Doctor.Name))
            Console.WriteLine(g.Key + ": " + g.Count() + " appointments");
    }

    static void LinqTop3Earning()
    {
        var top = hospital.Appointments.Where(a => a.IsCompleted).GroupBy(a => a.Doctor.Id)
            .Select(g => new { Doc = hospital.Doctors.First(d => d.Id == g.Key), Total = g.Sum(a => a.Doctor.ConsultationFee) })
            .OrderByDescending(x => x.Total).Take(3);
        foreach (var x in top) Console.WriteLine(x.Doc.Name + " " + x.Total);
    }

    static void LinqPatientsByDisease()
    {
        Console.Write("Disease: ");
        string d = Console.ReadLine();
        var list = hospital.Patients.Where(p => p.Disease.Equals(d, StringComparison.OrdinalIgnoreCase));
        foreach (var p in list) Console.WriteLine(p.Name);
    }

    static void LinqTotalRevenue()
    {
        double rev = hospital.Appointments.Where(a => a.IsCompleted).Sum(a => new AppointmentBill(a).CalculateBill());
        Console.WriteLine("Total revenue: " + rev);
    }
}
