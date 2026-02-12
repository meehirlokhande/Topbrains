-- Question
-- 6
-- MAX Salary
-- Description
-- Find Employees With Highest Salary per Department
-- Problem
-- Return employee details with the MAX salary department-wise.

-- Input
-- Table: Employees(Id, Name, Dept, Salary)

-- Output
-- Dept | Name | Salary

SELECT Department,Name,Salary FROM Employees WHERE Salary = (SELECT MAX(Salary) FROM Employees GROUP BY Department);
