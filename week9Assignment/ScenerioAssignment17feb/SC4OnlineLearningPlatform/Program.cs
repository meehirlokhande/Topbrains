#nullable disable
// Generic repository
class Repository<T> where T : class
{
    List<T> _items = new List<T>();

    public void Add(T item) => _items.Add(item);
    public IEnumerable<T> GetAll() => _items;
    public T GetById(Func<T, bool> match) => _items.FirstOrDefault(match);
}

class Course : IComparable<Course>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int MaxCapacity { get; set; }
    public int InstructorId { get; set; }
    public List<double> Ratings { get; } = new List<double>();

    public int CompareTo(Course other) => Name.CompareTo(other?.Name);
}

class Student
{
    public int Id { get; set; }
    public string Name { get; set; }
}

class Instructor
{
    public int Id { get; set; }
    public string Name { get; set; }
}

class Enrollment
{
    public int CourseId { get; set; }
    public int StudentId { get; set; }
    public DateTime EnrolledDate { get; set; }
}

class Assignment
{
    public int Id { get; set; }
    public int CourseId { get; set; }
    public int StudentId { get; set; }
    public DateTime Deadline { get; set; }
    public DateTime? SubmittedAt { get; set; }
}

class LateSubmissionException : Exception { public LateSubmissionException(string m) : base(m) { } }
class DuplicateEnrollmentException : Exception { public DuplicateEnrollmentException(string m) : base(m) { } }
class CapacityExceededException : Exception { public CapacityExceededException(string m) : base(m) { } }

class LearningPlatform
{
    public Repository<Course> Courses = new Repository<Course>();
    public Repository<Student> Students = new Repository<Student>();
    public Repository<Instructor> Instructors = new Repository<Instructor>();
    public List<Enrollment> Enrollments { get; } = new List<Enrollment>();
    public List<Assignment> Assignments { get; } = new List<Assignment>();

    public void Enroll(int courseId, int studentId)
    {
        if (Enrollments.Any(e => e.CourseId == courseId && e.StudentId == studentId))
            throw new DuplicateEnrollmentException("Already enrolled in this course");
        var course = Courses.GetById(c => (c as Course).Id == courseId) as Course;
        int count = Enrollments.Count(e => e.CourseId == courseId);
        if (count >= course.MaxCapacity) throw new CapacityExceededException("Course full");
        Enrollments.Add(new Enrollment { CourseId = courseId, StudentId = studentId, EnrolledDate = DateTime.Now });
    }

    public void SubmitAssignment(int assignmentId, DateTime submittedAt)
    {
        var a = Assignments.First(x => x.Id == assignmentId);
        if (submittedAt > a.Deadline) throw new LateSubmissionException("Submission after deadline");
        a.SubmittedAt = submittedAt;
    }
}

class Program
{
    static LearningPlatform platform = new LearningPlatform();

    static void Main(string[] args)
    {
        platform.Courses.Add(new Course { Id = 1, Name = "C# Basics", MaxCapacity = 100, InstructorId = 1 });
        platform.Courses.Add(new Course { Id = 2, Name = "SQL", MaxCapacity = 60, InstructorId = 2 });
        platform.Courses.Add(new Course { Id = 3, Name = "Web Dev", MaxCapacity = 80, InstructorId = 1 });
        platform.Courses.GetAll().Cast<Course>().First(c => c.Id == 1).Ratings.AddRange(new[] { 4.5, 4.0, 5.0 });
        platform.Courses.GetAll().Cast<Course>().First(c => c.Id == 2).Ratings.Add(4.2);

        platform.Students.Add(new Student { Id = 1, Name = "Anil" });
        platform.Students.Add(new Student { Id = 2, Name = "Bina" });
        platform.Students.Add(new Student { Id = 3, Name = "Chaya" });
        for (int i = 4; i <= 52; i++) platform.Students.Add(new Student { Id = i, Name = "Student" + i });
        platform.Instructors.Add(new Instructor { Id = 1, Name = "Prof. Kumar" });
        platform.Instructors.Add(new Instructor { Id = 2, Name = "Prof. Singh" });

        for (int i = 1; i <= 51; i++) platform.Enrollments.Add(new Enrollment { CourseId = 1, StudentId = i, EnrolledDate = DateTime.Now.AddDays(-i) });
        platform.Enrollments.Add(new Enrollment { CourseId = 2, StudentId = 1, EnrolledDate = DateTime.Now.AddDays(-2) });
        platform.Enrollments.Add(new Enrollment { CourseId = 2, StudentId = 2, EnrolledDate = DateTime.Now.AddDays(-1) });
        platform.Enrollments.Add(new Enrollment { CourseId = 3, StudentId = 1, EnrolledDate = DateTime.Now });
        platform.Enrollments.Add(new Enrollment { CourseId = 3, StudentId = 2, EnrolledDate = DateTime.Now });
        platform.Enrollments.Add(new Enrollment { CourseId = 3, StudentId = 3, EnrolledDate = DateTime.Now });

        while (true)
        {
            Console.WriteLine("\n--- Online Learning ---");
            Console.WriteLine("1. Enroll  2. Submit assignment");
            Console.WriteLine("3. Courses >50 students  4. Students in >3 courses  5. Most popular course  6. Avg rating  7. Top instructors  8. Custom sort courses  0. Exit");
            string c = Console.ReadLine();
            if (c == "0") break;
            try
            {
                if (c == "1") { Console.Write("CourseId StudentId: "); var s = Console.ReadLine().Split(); platform.Enroll(int.Parse(s[0]), int.Parse(s[1])); Console.WriteLine("Enrolled"); }
                else if (c == "2") { Console.Write("AssignmentId and date (yyyy-mm-dd): "); var s = Console.ReadLine().Split(); platform.SubmitAssignment(int.Parse(s[0]), DateTime.Parse(s[1])); Console.WriteLine("Submitted"); }
                else if (c == "3") LinqCoursesMore50();
                else if (c == "4") LinqStudentsMore3Courses();
                else if (c == "5") LinqMostPopular();
                else if (c == "6") LinqAvgRating();
                else if (c == "7") LinqTopInstructors();
                else if (c == "8") CustomSortCourses();
            }
            catch (Exception ex) { Console.WriteLine("Error: " + ex.Message); }
        }
    }

    static void LinqCoursesMore50()
    {
        var list = platform.Enrollments.GroupBy(e => e.CourseId).Where(g => g.Count() > 50).Select(g => platform.Courses.GetAll().Cast<Course>().First(c => c.Id == g.Key));
        foreach (var co in list) Console.WriteLine(co.Name);
    }

    static void LinqStudentsMore3Courses()
    {
        var list = platform.Enrollments.GroupBy(e => e.StudentId).Where(g => g.Count() > 3).Select(g => platform.Students.GetAll().Cast<Student>().First(s => s.Id == g.Key));
        foreach (var s in list) Console.WriteLine(s.Name);
    }

    static void LinqMostPopular()
    {
        var pop = platform.Enrollments.GroupBy(e => e.CourseId).OrderByDescending(g => g.Count()).First();
        var course = platform.Courses.GetAll().Cast<Course>().First(c => c.Id == pop.Key);
        Console.WriteLine("Most popular: " + course.Name);
    }

    static void LinqAvgRating()
    {
        foreach (var course in platform.Courses.GetAll().Cast<Course>())
        {
            double avg = course.Ratings.Count > 0 ? course.Ratings.Average() : 0;
            Console.WriteLine(course.Name + " avg rating: " + avg.ToString("F2"));
        }
    }

    static void LinqTopInstructors()
    {
        var courses = platform.Courses.GetAll().Cast<Course>();
        var instructors = platform.Instructors.GetAll().Cast<Instructor>();
        var enrollCount = platform.Enrollments.GroupBy(e => e.CourseId).ToDictionary(g => g.Key, g => g.Count());
        var joined = from c in courses
                     join i in instructors on c.InstructorId equals i.Id
                     select new { Instructor = i.Name, Enrollments = enrollCount.ContainsKey(c.Id) ? enrollCount[c.Id] : 0 };
        var byInst = joined.GroupBy(x => x.Instructor).Select(g => new { Name = g.Key, Total = g.Sum(x => x.Enrollments) }).OrderByDescending(x => x.Total);
        foreach (var x in byInst) Console.WriteLine(x.Name + " enrollments: " + x.Total);
    }

    static void CustomSortCourses()
    {
        var courses = platform.Courses.GetAll().Cast<Course>().ToList();
        courses.Sort();
        foreach (var co in courses) Console.WriteLine(co.Name);
    }
}
