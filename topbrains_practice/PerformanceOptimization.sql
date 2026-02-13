-- Performance optimization
-- Description
-- A query is performing table scans and running very slow.
-- Table size is 20 million rows.
-- Query:

-- SELECT * FROM Orders WHERE CustomerId = 1254 AND OrderDate > '2024-01-01'

-- Question:
-- Explain which indexes you will create and why (clustered / nonclustered / composite key).

CREATE CLUSTERED INDEX IX_Orders_CustomerId_OrderDate ON Orders(CustomerId, OrderDate);