CREATE TABLE [dbo].[Module]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [UserId] VARCHAR(10) NOT NULL, 
    [Date] DATE NULL, 
    [Title] VARCHAR(50) NULL, 
    [Hyperlink] VARCHAR(100) NULL, 
    CONSTRAINT [FK_Module_UserId] FOREIGN KEY ([UserId]) REFERENCES [User]([Id])
)
