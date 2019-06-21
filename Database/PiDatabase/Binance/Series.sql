CREATE TABLE [binance].[Series]
(
    [Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [SeriesParentId] INT NOT NULL 
        CONSTRAINT FK_SeriesParent_Series 
        FOREIGN KEY REFERENCES binance.SeriesParent(Id),
    [Currency] NVARCHAR(50) NOT NULL, 
    [Ammount] MONEY NOT NULL, 
    [AvgPrice] MONEY NOT NULL
)
