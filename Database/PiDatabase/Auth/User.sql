CREATE TABLE [auth].[User]
(
    [Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [DisplayName] NVARCHAR(100) NOT NULL, 
    [Password] VARBINARY(50) NOT NULL, 
    [Salt] VARBINARY(50) NOT NULL, 
    [Email] NVARCHAR(256) NULL, 
    --If this is NULL, then User is not active, regardless of ActiveTo
    [ActiveFromUtc] DATETIME NULL, 
    --If ActiveFrom is not NULL and ActiveTo is null, then User is active infinitely
    [ActiveToUtc] DATETIME NULL,
    [CreatedDateUtc] DATETIME NOT NULL,
    [LastLoginUtc] DATETIME NULL,
)
