CREATE TABLE [dbo].[User]
(
	[Id] VARCHAR(10) NOT NULL PRIMARY KEY, 
    [Name] VARCHAR(50) NULL,
    [Email] VARCHAR(50) NULL, 
    [Password] VARCHAR(50) NULL, 
    [AccessLevel] INT NULL, 
    [Address] VARCHAR(100) NULL, 
    [Salary] MONEY NULL, 
    [VacationDays] INT NULL, 
    [SickDays] INT NULL 
)
