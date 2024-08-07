SELECT TOP(1) [p].[Id], [p].[BirthDate], [p].[Earnings], [p].[FatherId], [p].[Forename], [p].[MotherId], [p].[PartnerId], [p].[Surname], [p].[Gender]
FROM [Persons] AS [p]
ORDER BY (
    SELECT COUNT(*)
    FROM [Persons] AS [p0]
    WHERE [p].[Id] = [p0].[PartnerId]) DESC

-- Person with the most female children query

SELECT TOP(1) [p].[Id], [p].[BirthDate], [p].[Earnings], [p].[FatherId], [p].[Forename], [p].[MotherId], [p].[PartnerId], [p].[Surname], [p].[Gender]
FROM [Persons] AS [p]
ORDER BY (
    SELECT COUNT(*)
    FROM [Persons] AS [p0]
    WHERE [p].[Id] = [p0].[PartnerId] AND [p0].[Gender] = N'Female') DESC