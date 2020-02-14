CREATE TABLE [auth].[User]
(
    [Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [Username] NVARCHAR(50) NOT NULL, 
    [DisplayName] NVARCHAR(50) NOT NULL, 
    [Password] NVARCHAR(50) NOT NULL, 
    [CreatedDate] DATETIME NOT NULL 
)
