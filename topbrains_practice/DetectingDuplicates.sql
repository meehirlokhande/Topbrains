-- Question
-- 5
-- Detecting Duplicate
-- Description
-- Detect Duplicate User Emails
-- Input
-- Users table with Email column

-- Output
-- List of duplicate emails + count

SELECT Email,COUNT(Email) as DuplicateCount FROM Users GROUP BY Email HAVING COUNT(Email)>1;
