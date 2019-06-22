CREATE TABLE [otodom].[Flat]
(
    [Id] INT NOT NULL PRIMARY KEY,
    [OtoDomId] nvarchar(7) NOT NULL UNIQUE,
    [Surface] DECIMAL(9,2) NOT NULL, 
    [TotalPrice] MONEY NOT NULL, 
    [Rooms] TINYINT NULL,
    [Rent] SMALLMONEY NULL, 
    [Floor] TINYINT NULL, 
    [FloorsNo] TINYINT NULL, 
    [ConstructionYear] SMALLINT NULL,
    [MarketId] INT NULL
        CONSTRAINT FK_Market_Flat 
        FOREIGN KEY REFERENCES [otodom].[Market],
    [HeatingId] INT NULL
        CONSTRAINT FK_Heating_Flat 
        FOREIGN KEY REFERENCES [otodom].[Heating],
    [LocationId] INT NULL
        CONSTRAINT FK_Location_Flat 
        FOREIGN KEY REFERENCES [otodom].[Location],
    [FormOfPropertyId] INT NULL
        CONSTRAINT FK_FormOfProperty_Flat 
        FOREIGN KEY REFERENCES [otodom].[FormOfProperty],
    [TypeId] INT NULL
        CONSTRAINT FK_TypeOfBuilding_Flat 
        FOREIGN KEY REFERENCES [otodom].[TypeOfBuilding],
    [Url] VARCHAR(200) NULL
)
