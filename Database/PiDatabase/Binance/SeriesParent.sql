CREATE TABLE [binance].[SeriesParent]
(
    [Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [FetchedDate] DATETIME NOT NULL, 
    [Total] MONEY NOT NULL
)
