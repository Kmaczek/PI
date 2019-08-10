CREATE TABLE [otodom].[FlatAdditionalInfo]
(
    [Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [FlatId] INT NOT NULL
        CONSTRAINT FK_Flat_FlatAdditionalInfo
        FOREIGN KEY REFERENCES [otodom].[Flat],
    [AdditionalInfoId] INT NOT NULL
        CONSTRAINT FK_AdditionalInfo_FlatAdditionalInfo
        FOREIGN KEY REFERENCES [otodom].[AdditionalInfo]
)
