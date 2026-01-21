using System;

abstract class Employee{
    public abstract decimal CalculatePay();
}

class HourlyEmployee:Employee{
    private decimal rate;
    private decimal hours;
    
    public HourlyEmployee(decimal rate, decimal hours){
        this.rate = rate;
        this.hours = hours;
    }

    public override decimal CalculatePay(){
        return rate*hours;
    }
}

class SalariedEmployee:Employee{
    private decimal monthlySalary;

    public SalariedEmployee(decimal monthlySalary){
        this.monthlySalary = monthlySalary;
    }

    public override decimal CalculatePay(){
        return monthlySalary;
    }
}

class CommissionEmployee:Employee{

    private decimal baseSalary;
    private decimal commission;

    public CommissionEmployee(decimal baseSalary, decimal commission){
        this.baseSalary = baseSalary;
        this.commission = commission;
    }

    public override decimal CalculatePay(){
        return baseSalary + commission;
    } 
}


class InheritanceAndPolymorphism{

    public static decimal ComputeTotalPayroll(string[] employees){
        decimal totalPay = 0;
        foreach(var emp in employees){
            string[] parts = emp.Split(' ');
            Employee employee = parts[0] switch{
                "H" => new HourlyEmployee(decimal.Parse(parts[1]), decimal.Parse(parts[2])),
                "S" => new SalariedEmployee(decimal.Parse(parts[1])),
                "C" => new CommissionEmployee(decimal.Parse(parts[1]), decimal.Parse(parts[2])),
            };
            totalPay += employee.CalculatePay();        
        }
        return Math.Round(totalPay,2);
    }

    public static void Main(string[] args){
        string[] employees = new string[]{
            "H 25.50 40",
            "S 5000",
            "C 1000 0.05"
        };

        decimal totalPay = ComputeTotalPayroll(employees);
        Console.WriteLine($"Total Payroll: {totalPay}");
    }
}