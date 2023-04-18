CREATE TABLE [dbo].[Requests]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [UserId] VARCHAR(10) NOT NULL, 
    [From] VARCHAR(50) NULL, 
    [To] VARCHAR(50) NULL, 
    [Type] VARCHAR(50) NULL, 
    [StartDate] DATETIME NULL, 
    [EndDate] DATETIME NULL, 
    CONSTRAINT [FK_Requests_UserId] FOREIGN KEY ([UserId]) REFERENCES [User]([Id])
)
