CREATE TABLE [dbo].[WorkHours]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [UserId] VARCHAR(10) NULL, 
    [StartTime] DATETIME NULL, 
    [EndTime] DATETIME NULL, 
    [Date] DATE NULL, 
    [Function] VARCHAR(20) NULL, 
    CONSTRAINT [FK_WorkHours_UserId] FOREIGN KEY ([UserId]) REFERENCES [User]([Id])
)
