CREATE TABLE Sales_Raw

(

    OrderID INT,

    OrderDate VARCHAR(20),

    CustomerName VARCHAR(100),

    CustomerPhone VARCHAR(20),

    CustomerCity VARCHAR(50),

    ProductNames VARCHAR(200),   -- Multiple products comma-separated

    Quantities VARCHAR(100),     -- Multiple quantities comma-separated

    UnitPrices VARCHAR(100),     -- Multiple prices comma-separated

    SalesPerson VARCHAR(100)

);


INSERT INTO Sales_Raw VALUES

(101, '2024-01-05', 'Ravi Kumar', '9876543210', 'Chennai',

 'Laptop,Mouse', '1,2', '55000,500', 'Anitha'),

 

(102, '2024-01-06', 'Priya Sharma', '9123456789', 'Bangalore',

 'Keyboard,Mouse', '1,1', '1500,500', 'Anitha'),

 

(103, '2024-01-10', 'Ravi Kumar', '9876543210', 'Chennai',

 'Laptop', '1', '54000', 'Suresh'),

 

(104, '2024-02-01', 'John Peter', '9988776655', 'Hyderabad',

 'Monitor,Mouse', '1,1', '12000,500', 'Anitha'),

 

(105, '2024-02-10', 'Priya Sharma', '9123456789', 'Bangalore',

 'Laptop,Keyboard', '1,1', '56000,1500', 'Suresh');


 /* 

 Problems Observed In Sales_Raw Table:

1) Repeating Groups- ProductNames, Quantities, UnitPrices contain multiple values (Violates 1NF)
2) Customer Data Repeated
3)SalesPersons Data mixed in order data ( violates 3NF)
 
 */


/*   Normalized Tables  */

/* Customer Table:  */

CREATE TABLE Customers (
    CustomerID INT IDENTITY PRIMARY KEY,
    CustomerName VARCHAR(100),
    CustomerPhone VARCHAR(20),
    CustomerCity VARCHAR(50)
);

/* SalesPersons Table   */


CREATE TABLE SalesPersons (
    SalesPersonID INT IDENTITY PRIMARY KEY,
    SalesPersonName VARCHAR(100)
);

/* Products Table  */

CREATE TABLE Products (
    ProductID INT IDENTITY PRIMARY KEY,
    ProductName VARCHAR(100),
    UnitPrice INT
);


/* Orders Table  */

CREATE TABLE Orders (
    OrderID INT PRIMARY KEY,
    OrderDate DATE,
    CustomerID INT,
    SalesPersonID INT,
    FOREIGN KEY (CustomerID) REFERENCES Customers(CustomerID),
    FOREIGN KEY (SalesPersonID) REFERENCES SalesPersons(SalesPersonID)
);

/* OrderDetails Table  */

CREATE TABLE OrderDetails (
    OrderDetailID INT IDENTITY PRIMARY KEY,
    OrderID INT,
    ProductID INT,
    Quantity INT,
    FOREIGN KEY (OrderID) REFERENCES Orders(OrderID),
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID)
);



/* QUESTION 2 – THIRD HIGHEST TOTAL SALES (PER ORDER)

Soultion:

*/

WITH OrderTotals AS (
    SELECT 
        OrderID,
        SUM(CAST(q.value AS INT) * CAST(p.value AS INT)) AS TotalSales
    FROM Sales_Raw
    CROSS APPLY STRING_SPLIT(Quantities, ',') q
    CROSS APPLY STRING_SPLIT(UnitPrices, ',') p
    GROUP BY OrderID
),
RankedOrders AS (
    SELECT *,
           DENSE_RANK() OVER (ORDER BY TotalSales DESC) AS rnk
    FROM OrderTotals
)
SELECT OrderID, TotalSales
FROM RankedOrders
WHERE rnk = 3;


/*  

QUESTION 3 – SALES PERSONS WITH TOTAL SALES > ₹60,000

Solution:

*/

SELECT 
    SalesPerson,
    SUM(CAST(q.value AS INT) * CAST(p.value AS INT)) AS TotalSales
FROM Sales_Raw
CROSS APPLY STRING_SPLIT(Quantities, ',') q
CROSS APPLY STRING_SPLIT(UnitPrices, ',') p
GROUP BY SalesPerson
HAVING SUM(CAST(q.value AS INT) * CAST(p.value AS INT)) > 60000;


/*

QUESTION 4 – CUSTOMERS SPENDING ABOVE AVERAGE

*/

WITH CustomerTotals AS (
    SELECT 
        CustomerName,
        SUM(CAST(q.value AS INT) * CAST(p.value AS INT)) AS TotalSpent
    FROM Sales_Raw
    CROSS APPLY STRING_SPLIT(Quantities, ',') q
    CROSS APPLY STRING_SPLIT(UnitPrices, ',') p
    GROUP BY CustomerName
)
SELECT *
FROM CustomerTotals
WHERE TotalSpent > (
    SELECT AVG(TotalSpent) FROM CustomerTotals
);

/*

QUESTION 5 – STRING & DATE FUNCTIONS

Solutions:

*/

SELECT 
    UPPER(CustomerName) AS CustomerName,
    MONTH(CONVERT(DATE, OrderDate)) AS OrderMonth
FROM Sales_Raw
WHERE 
    YEAR(CONVERT(DATE, OrderDate)) = 2026
    AND MONTH(CONVERT(DATE, OrderDate)) = 1;
