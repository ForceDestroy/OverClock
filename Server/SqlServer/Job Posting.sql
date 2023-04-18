CREATE TABLE [dbo].[Job Posting]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [Title] VARCHAR(50) NULL, 
    [Author] VARCHAR(50) NULL, 
    [Date] DATE NULL, 
    [Salary] MONEY NULL, 
    [Description] TEXT NULL
)
