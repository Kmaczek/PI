CREATE TABLE [auth].[UserRoles]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[UserId] INT NOT NULL 
        CONSTRAINT FK_UserRoles_User
        FOREIGN KEY REFERENCES auth.[User](Id),
	[RoleId] INT NOT NULL 
        CONSTRAINT FK_UserRoles_Role
        FOREIGN KEY REFERENCES auth.[Role](Id),
)
