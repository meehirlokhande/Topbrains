-- Identify the orphan records
-- Description
-- You suspect data inconsistency between Orders and OrderItems.
-- There are OrderItems whose OrderId does not exist in Orders table.


-- Question:
-- Write a query to find orphan records using LEFT JOIN or NOT EXISTS.

SELECT * FROM OrderItems LEFT JOIN Orders ON OrderItems.OrderId = Orders.OrderId WHERE Orders.OrderId is NULL;
