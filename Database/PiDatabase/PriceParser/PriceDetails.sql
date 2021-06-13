CREATE TABLE [price].[PriceDetails]
(
    [Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    [Title] VARCHAR(500) NULL,
    [RetailerNo] VARCHAR(100) NULL,
    [CreatedDate] DATETIME NOT NULL,
)
