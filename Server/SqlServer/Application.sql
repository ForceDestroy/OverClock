CREATE TABLE [dbo].[Application]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [Name] VARCHAR(50) NULL,
    [EmployeeId] VARCHAR(10) NULL,
    [Referral] CHAR(1) NOT NULL,
    [Education] VARCHAR(50) NULL, 
    [Experience] TEXT NULL, 
    [Skill Set] TEXT NULL, 
    [Date] DATE NULL
)
