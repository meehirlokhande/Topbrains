using System;
using System.Collections.Generic;

class Student{
    public string Name{get;set;}
    public int Age{get;set;}
    public int Marks{get;set;}
   
    public Student(string name, int age, int marks){
        Name = name;
        Age = age;
        Marks = marks;   
    }
    
}

class Sorting:IComparer<Student>{
    public int Compare(Student x,Student y){
        if(x.Marks!=y.Marks){
            return y.Marks.CompareTo(x.Marks);
        }
        return x.Age.CompareTo(y.Age);
    }
}

class CustomSorting{
    public static void Main(string[] args){
        List<Student> students = new List<Student>();
        students.Add(new Student("Meehir",23,90));
        students.Add(new Student("Nilesh",21,75));
        students.Add(new Student("Shivam",22,94));
        students.Add(new Student("Pratyush",25,98));
        students.Add(new Student("Rajiv",22,90));
        students.Add(new Student("Mohit",23,80));

        students.Sort(new Sorting());
        foreach(Student student in students){
            Console.WriteLine(student.Name + " " + student.Age + " " + student.Marks);
        }   
    

    }
}