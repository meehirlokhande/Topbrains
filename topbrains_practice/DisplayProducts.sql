-- Display Products
-- Description
-- Display Products Not Sold
-- Input
-- Products table
-- Sales table

-- Output
-- Products that have never appeared in Sales.

SELECT * FROM Products WHERE PRODUCT_ID NOT IN (SELECT PRODUCT_ID FROM Sales);