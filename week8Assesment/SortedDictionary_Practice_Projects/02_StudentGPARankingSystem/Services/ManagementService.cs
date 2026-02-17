using System;
using System.Collections.Generic;
using Domain;
using Exceptions;

namespace Services
{
    public class ManagementService
    {
        private SortedDictionary<double, List<Student>> _data =
            new SortedDictionary<double, List<Student>>(Comparer<double>.Create((a, b) => b.CompareTo(a)));

        public void AddStudent(Student student)
        {
            if (student.GPA < 0 || student.GPA > 10)
                throw new InvalidGPAException("Invalid GPA");

            foreach (var kvp in _data)
            {
                foreach (var s in kvp.Value)
                {
                    if (s.Id == student.Id)
                        throw new DuplicateStudentException("Duplicate Student");
                }
            }

            if (!_data.ContainsKey(student.GPA))
                _data[student.GPA] = new List<Student>();

            _data[student.GPA].Add(student);
        }

        public void GetAllStudents()
        {
            foreach (var kvp in _data)
            {
                foreach (var s in kvp.Value)
                {
                    Console.WriteLine($"Details: {s.Id} {s.Name} {s.GPA}");
                }
            }
        }

        public void UpdateGPA(string id, double newGPA)
        {
            if (newGPA < 0 || newGPA > 10)
                throw new InvalidGPAException("Invalid GPA");

            foreach (var kvp in _data)
            {
                foreach (var s in kvp.Value)
                {
                    if (s.Id == id)
                    {
                        double oldGPA = s.GPA;
                        kvp.Value.Remove(s);
                        if (kvp.Value.Count == 0)
                            _data.Remove(oldGPA);

                        s.GPA = newGPA;
                        if (!_data.ContainsKey(newGPA))
                            _data[newGPA] = new List<Student>();
                        _data[newGPA].Add(s);

                        Console.WriteLine($"Updated GPA: {newGPA}");
                        return;
                    }
                }
            }

            throw new StudentNotFoundException("Student Not Found");
        }
    }
}
