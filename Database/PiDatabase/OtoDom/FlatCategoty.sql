﻿CREATE TABLE [otodom].[FlatCategoty]
(
    [Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [Name] NVARCHAR(50) NOT NULL,
    [LowerBand] MONEY NOT NULL,
    [UpperBand] MONEY NOT NULL,
)
