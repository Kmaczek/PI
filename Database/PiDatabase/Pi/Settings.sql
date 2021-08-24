CREATE TABLE [pi].[Settings]
(
    [Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [Name] NVARCHAR(100) NOT NULL,
    [Value] NVARCHAR(MAX) NOT NULL, 
    [Description] NVARCHAR(300) NULL
)
