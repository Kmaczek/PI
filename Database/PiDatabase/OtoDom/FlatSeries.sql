CREATE TABLE [otodom].[FlatSeries]
(
    [Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [DateFetched] datetime NOT NULL, 
    [AvgPricePerMeter] MONEY NOT NULL, 
    [AvgPrice] MONEY NOT NULL, 
    [CategoryId] INT NULL
        CONSTRAINT FK_Category_FlatSeries 
        FOREIGN KEY REFERENCES [otodom].[FlatCategoty],
    [Amount] INT NOT NULL,
    [BiggestId] INT NULL
        CONSTRAINT FK_Biggest_FlatSeries 
        FOREIGN KEY REFERENCES [otodom].[Flat],
    [SmallestId] INT NULL
        CONSTRAINT FK_Smallest_FlatSeries 
        FOREIGN KEY REFERENCES [otodom].[Flat],
    [CheapestId] INT NULL
        CONSTRAINT FK_Cheapest_FlatSeries 
        FOREIGN KEY REFERENCES [otodom].[Flat],
    [MostExpensiveId] INT NULL
        CONSTRAINT FK_MostExpensive_FlatSeries 
        FOREIGN KEY REFERENCES [otodom].[Flat],
    [BestValueId] INT NULL
        CONSTRAINT FK_BestValue_FlatSeries 
        FOREIGN KEY REFERENCES [otodom].[Flat],
)
