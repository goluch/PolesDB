SELECT TOP(1) [p].[Id], [p].[Forename], [p].[Surname]
FROM [Persons] AS [p]
ORDER BY (
    SELECT COUNT(*)
    FROM [Persons] AS [p0]
    WHERE [p].[Id] = [p0].[PartnerId]) DESC

-- Person with the most female children query

SELECT TOP(1) [p].[Id], [p].[Forename], [p].[Surname]
FROM [Persons] AS [p]
ORDER BY (
    SELECT COUNT(*)
    FROM [Persons] AS [p0]
    WHERE [p].[Id] = [p0].[PartnerId] AND [p0].[Gender] = N'Female') DESC

SELECT COUNT(*) FROM Employments
SELECT COUNT(*)
              FROM Employments AS e
              WHERE e.Contract = N'Employment Contract'

SELECT [p].[Id], [p].[Forename], [p].[Surname]
FROM [Persons] AS [p]
WHERE [p].[Id] = 1