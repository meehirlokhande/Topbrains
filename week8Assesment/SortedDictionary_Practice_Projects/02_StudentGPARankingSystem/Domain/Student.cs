namespace Domain
{
    public class Student
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public double GPA { get; set; }

        public Student(string id, string name, double gpa)
        {
            Id = id;
            Name = name;
            GPA = gpa;
        }
    }
}
