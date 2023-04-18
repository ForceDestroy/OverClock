CREATE TABLE [dbo].[PaySlip]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [UserId] VARCHAR(10) NULL, 
    [StartDate] DATETIME NULL, 
    [EndDate] DATETIME NULL, 
    [GrossAmount] MONEY NULL, 
    [AmountAccumulated] MONEY NULL, 
    [FederalTax] MONEY NULL, 
    [ProvincialTax] MONEY NULL, 
    [NetAmount] MONEY NULL, 
    [HoursAccumulated] INT NULL, 
    [HoursCurrent] INT NULL, 
    CONSTRAINT [FK_PaySlip_UserId] FOREIGN KEY ([UserId]) REFERENCES [User]([Id])
)
