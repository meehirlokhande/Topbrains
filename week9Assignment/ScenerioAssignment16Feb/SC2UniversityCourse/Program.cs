using System;


using System.Collections.Generic;


using System.Linq;


public interface IStudent


{


    int StudentId { get; }


    string Name { get; }


    int Semester { get; }


}


public interface ICourse


{


    string CourseCode { get; }

    string Title { get; }


    int MaxCapacity { get; }


    int Credits { get; }


}


public class EnrollmentSystem<TStudent, TCourse>


where TStudent : IStudent


where TCourse : ICourse


{


    private readonly Dictionary<TCourse, List<TStudent>> _enrollments = new();





    public bool EnrollStudent(TStudent student, TCourse course)


    {


        if (!_enrollments.ContainsKey(course))


            _enrollments[course] = new List<TStudent>();





        var students = _enrollments[course];








        if (students.Count >= course.MaxCapacity)


        {

            Console.WriteLine("Enrollment failed: Course is at full capacity.");


            return false;


        }





        if (students.Any(s => s.StudentId == student.StudentId))


        {


            Console.WriteLine("Enrollment failed: Student already enrolled.");


            return false;


        }





        if (course is LabCourse labCourse)


        {


            if (student.Semester < labCourse.RequiredSemester)


            {


                Console.WriteLine("Enrollment failed: Prerequisite semester not met.");


                return false;


            }


        }





        students.Add(student);

        Console.WriteLine("Enrollment successful.");


        return true;


    }





    public IReadOnlyList<TStudent> GetEnrolledStudents(TCourse course)


    {


        if (_enrollments.TryGetValue(course, out var students))


            return students.AsReadOnly();





        return new List<TStudent>().AsReadOnly();


    }





    public IEnumerable<TCourse> GetStudentCourses(TStudent student)


    {


        return _enrollments


        .Where(e => e.Value.Any(s => s.StudentId == student.StudentId))


        .Select(e => e.Key);


    }





    public int CalculateStudentWorkload(TStudent student)

    {


        return GetStudentCourses(student).Sum(c => c.Credits);


    }


}





public class EngineeringStudent : IStudent


{


    public int StudentId { get; set; }


    public string Name { get; set; }


    public int Semester { get; set; }


    public string Specialization { get; set; }


}





public class LabCourse : ICourse


{


    public string CourseCode { get; set; }


    public string Title { get; set; }


    public int MaxCapacity { get; set; }


    public int Credits { get; set; }


    public string LabEquipment { get; set; }

    public int RequiredSemester { get; set; }


}






public class GradeBook<TStudent, TCourse>


where TStudent : IStudent


where TCourse : ICourse


{


    private readonly EnrollmentSystem<TStudent, TCourse> _enrollmentSystem;


    private readonly Dictionary<(TStudent, TCourse), double> _grades = new();





    public GradeBook(EnrollmentSystem<TStudent, TCourse> enrollmentSystem)


    {


        _enrollmentSystem = enrollmentSystem;


    }





    public void AddGrade(TStudent student, TCourse course, double grade)


    {


        if (grade < 0 || grade > 100)


            throw new ArgumentException("Grade must be between 0 and 100.");

        var enrolled = _enrollmentSystem


        .GetStudentCourses(student)


        .Any(c => c.Equals(course));





        if (!enrolled)


            throw new InvalidOperationException("Student not enrolled in course.");





        _grades[(student, course)] = grade;


    }





    public double? CalculateGPA(TStudent student)


    {


        var studentGrades = _grades


        .Where(g => g.Key.Item1.StudentId == student.StudentId)


        .ToList();





        if (!studentGrades.Any())


            return null;





        double totalPoints = 0;

        int totalCredits = 0;





        foreach (var entry in studentGrades)


        {


            var course = entry.Key.Item2;


            totalPoints += entry.Value * course.Credits;


            totalCredits += course.Credits;


        }





        return totalPoints / totalCredits;


    }





    public (TStudent student, double grade)? GetTopStudent(TCourse course)


    {


        var courseGrades = _grades


        .Where(g => g.Key.Item2.Equals(course))


        .OrderByDescending(g => g.Value)


        .FirstOrDefault();





        if (courseGrades.Equals(default(KeyValuePair<(TStudent, TCourse), double>)))

            return null;





        return (courseGrades.Key.Item1, courseGrades.Value);


    }


}





public class Program


{


    public static void Main()


    {


        var enrollment = new EnrollmentSystem<EngineeringStudent, LabCourse>();


        var gradebook = new GradeBook<EngineeringStudent, LabCourse>(enrollment);





        // a) Students


        var s1 = new EngineeringStudent { StudentId = 1, Name = "Rahul", Semester = 3, Specialization = "CSE" };


        var s2 = new EngineeringStudent { StudentId = 2, Name = "Priya", Semester = 2, Specialization = "ECE" };


        var s3 = new EngineeringStudent
        {
            StudentId = 3,
            Name = "Arjun",
            Semester = 4,
            Specialization =
        "ME"
        };





        // b) Courses

        var c1 = new LabCourse { CourseCode = "LAB101", Title = "Physics Lab", Credits = 3, MaxCapacity = 2, RequiredSemester = 2 };


        var c2 = new LabCourse { CourseCode = "LAB201", Title = "Advanced Robotics Lab", Credits = 4, MaxCapacity = 1, RequiredSemester = 4 };





        // Successful enrollments


        enrollment.EnrollStudent(s1, c1);


        enrollment.EnrollStudent(s2, c1);





        // Fail: capacity


        enrollment.EnrollStudent(s3, c1);





        // Fail: prerequisite


        enrollment.EnrollStudent(s2, c2);





        // Success


        enrollment.EnrollStudent(s3, c2);





        // Grades


        gradebook.AddGrade(s1, c1, 85);


        gradebook.AddGrade(s2, c1, 90);

        gradebook.AddGrade(s3, c2, 95);





        Console.WriteLine($"Rahul GPA: {gradebook.CalculateGPA(s1)}");


        Console.WriteLine($"Priya GPA: {gradebook.CalculateGPA(s2)}");


        Console.WriteLine($"Arjun GPA: {gradebook.CalculateGPA(s3)}");





        var top = gradebook.GetTopStudent(c1);


        if (top != null)


            Console.WriteLine($"Top student in {c1.Title}: {top.Value.student.Name} ({top.Value.grade})");


    }


}


