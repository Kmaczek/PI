CREATE TABLE [price].[PriceSeries]
(
    [Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [ParserId] INT
        CONSTRAINT FK_Product_ParserSeries 
        FOREIGN KEY REFERENCES [price].[Product] NOT NULL,
    [PriceDetailsId] INT
        CONSTRAINT FK_PriceDetails_ParserSeries 
        FOREIGN KEY REFERENCES [price].[PriceDetails] NOT NULL,
    [Price] MONEY NOT NULL, 
    [CreatedDate] DATETIME NOT NULL
)
