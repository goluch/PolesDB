--SELECT * FROM [Persons] WHERE Id = 0;
--SELECT MIN(Earnings) FROM Persons;
--SELECT COUNT(*) AS [Number of children], ParentID FROM Persons GROUP BY ParentID ORDER BY [Number of children] DESC;

-- Person with the most children query

--SELECT TOP(1) [p].[Id], [p].[BirthDate], [p].[Earnings], [p].[Forename], [p].[ParentId], [p].[PartnerId], [p].[Surname], [p].[Gender]
--FROM [Persons] AS [p]
--ORDER BY (
--    SELECT COUNT(*)
--    FROM [Persons] AS [p0]
--    WHERE [p].[Id] = [p0].[ParentId]) DESC



