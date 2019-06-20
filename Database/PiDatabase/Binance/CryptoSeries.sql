CREATE TABLE [binance].[CryptoSeries]
(
    [Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [SeriesParentId] INT NOT NULL 
        CONSTRAINT FK_SeriesParent_CryptoSeries 
        FOREIGN KEY REFERENCES binance.SeriesParent(Id),
    [Currency] NVARCHAR(50) NOT NULL, 
    [Ammount] DECIMAL NOT NULL, 
    [AvgPrice] DECIMAL NOT NULL
)
