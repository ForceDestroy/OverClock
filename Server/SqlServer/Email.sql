CREATE TABLE [dbo].[Email]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [UserId] VARCHAR(10) NULL, 
    [From] VARCHAR(50) NULL, 
    [To] VARCHAR(50) NULL, 
    [Subject] VARCHAR(50) NULL, 
    [Body] TEXT NULL, 
    CONSTRAINT [FK_Email_UserId] FOREIGN KEY ([UserId]) REFERENCES [User]([Id])
)
