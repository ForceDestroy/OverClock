CREATE TABLE [dbo].[Schedule]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [UserId] VARCHAR(10) NULL,
	[StartTime] DATETIME NULL, 
    [EndTime] DATETIME NULL, 
    [Date] DATE NULL, 
    CONSTRAINT [FK_Schedule_UserId] FOREIGN KEY ([UserId]) REFERENCES [User]([Id])
)
