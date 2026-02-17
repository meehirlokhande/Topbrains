namespace Domain
{
    public class Patient
    {
        public string PatientId { get; set; }
        public string Name { get; set; }
        public int SeverityLevel { get; set; }

        public Patient(string patientId, string name, int severityLevel)
        {
            PatientId = patientId;
            Name = name;
            SeverityLevel = severityLevel;
        }
    }
}
