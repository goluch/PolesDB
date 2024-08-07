SELECT * FROM [Persons] WHERE Id = 0;
SELECT MIN(Earnings) FROM Persons;
SELECT COUNT(*) AS [Number of children], ParentID FROM Persons GROUP BY ParentID ORDER BY [Number of children] DESC;

-- Person with the most children query

SELECT TOP(1) [p].[Id], [p].[Forename], [p].[Surname]
FROM [Persons] AS [p]
ORDER BY (
    SELECT COUNT(*)
    FROM [Persons] AS [p0]
    WHERE [p].[Id] = [p0].[PartnerId]) DESC

-- Person with the most female children query

SELECT TOP(1) [p].[Id], [p].[BirthDate], [p].[Earnings], [p].[Forename], [p].[ParentId], [p].[PartnerId], [p].[Surname], [p].[Gender]
FROM [Persons] AS [p]
ORDER BY (
    SELECT COUNT(*)
    FROM [Persons] AS [p0]
    WHERE [p].[Id] = [p0].[ParentId] AND [p0].[Gender] = N'Female') DESC

 SELECT COUNT(*) FROM Employments

 SELECT COUNT(*)
              FROM Employments AS e
              WHERE e.Contract = N'Employment Contract'

 SELECT COUNT(*)
              FROM Employments AS e
              WHERE e.Contract = N'Mandate Contract'

-- The poorest max 1 generation family, query

SELECT TOP(1) [p].[Id], [p].[BirthDate], [p].[Earnings], [p].[Forename], [p].[ParentId], [p].[PartnerId], [p].[Surname], [p].[Gender]
FROM [Persons] AS [p]
LEFT JOIN [Persons] AS [p0] ON [p].[PartnerId] = [p0].[Id]
ORDER BY [p].[Earnings] + CASE
    WHEN [p0].[Id] IS NULL THEN 0
    ELSE [p0].[Earnings]
END

-- The poorest max 2 generation family, query

SELECT TOP(1) [p].[Id], [p].[BirthDate], [p].[Earnings], [p].[Forename], [p].[ParentId], [p].[PartnerId], [p].[Surname], [p].[Gender]
FROM [Persons] AS [p]
LEFT JOIN [Persons] AS [p0] ON [p].[PartnerId] = [p0].[Id]
ORDER BY [p].[Earnings] + CASE
    WHEN [p0].[Id] IS NULL THEN 0
    ELSE [p0].[Earnings] + (
        SELECT COALESCE(SUM([p1].[Earnings] + CASE
            WHEN [p2].[Id] IS NULL THEN 0
            ELSE [p2].[Earnings]
        END), 0)
        FROM [Persons] AS [p1]
        LEFT JOIN [Persons] AS [p2] ON [p1].[PartnerId] = [p2].[Id]
        WHERE [p].[Id] = [p1].[ParentId])
END