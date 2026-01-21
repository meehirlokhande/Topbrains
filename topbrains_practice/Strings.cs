using System;

interface IHasArea{
    double Area();
}

abstract class Shapes:IHasArea{
    public abstract double Area();
}

class Circle:Shapes{
    private double radius;

    public Circle(double radius){
        this.radius = radius;
    }

    public override double Area(){
        return Math.PI*radius*radius;
    }
}

class Rectangle:Shapes{
    private double width;
    private double height;

    public Rectangle(double width,double height){
        this.width = width;
        this.height = height;
    }

    public override double Area(){
        return width*height;
    }
}

class Triangle:Shapes{

    private double baseLength;
    private double height;

    public Triangle(double b,double h){
        this.baseLength = b;
        this.height = h;
    }

    public override double Area(){
        return 0.5*baseLength*height;
    }
}



class Strings{
    public static void Main(string[] args){
        string[] shapes = new string[]{
            "C 5",
            "R 4 6",
            "T 3 8"
        };
        double totalArea = 0;
        foreach(var s in shapes){
            string[] parts = s.Split(' ');
            Shapes shape = parts[0] switch{
                "C" => new Circle(double.Parse(parts[1])),
                "R" => new Rectangle(double.Parse(parts[1]), double.Parse(parts[2])),
                "T" => new Triangle(double.Parse(parts[1]), double.Parse(parts[2])),
            };  
            totalArea += shape.Area();
        };
        Console.WriteLine($"Total Area: {Math.Round(totalArea,2)}");

    }
}