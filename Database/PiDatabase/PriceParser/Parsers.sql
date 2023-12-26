CREATE TABLE [price].[Parser]
(
    [Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    [Uri] VARCHAR(500) NOT NULL, 
    [ParserTypeId] INT NOT NULL
        CONSTRAINT FK_ParserType_Parser 
        FOREIGN KEY REFERENCES [price].[ParserType],
    [Params] NVARCHAR(MAX), 
    [Track] BIT NOT NULL, 
    [CreatedDate] DATETIME NOT NULL, 
    [UpdatedDate] DATETIME NOT NULL,
    [ActiveFrom] DATETIME NOT NULL,
    [ActiveTo] DATETIME NOT NULL, 
    [LatestPriceDetailId] INT NULL
        CONSTRAINT FK_PriceDetail_Parser 
        FOREIGN KEY REFERENCES [price].[PriceDetails]
)
