
IF NOT EXISTS (SELECT * FROM price.ParserType WHERE [Name] = 'XcomPriceParser')
BEGIN
	SET IDENTITY_INSERT price.ParserType ON;
	INSERT INTO price.ParserType ([Id], [Name]) VALUES (1, 'XcomPriceParser')
	SET IDENTITY_INSERT price.ParserType OFF;
END


IF NOT EXISTS (SELECT * FROM price.ParserType WHERE [Name] = 'FriscoPriceParser')
BEGIN
	SET IDENTITY_INSERT price.ParserType ON;
	INSERT INTO price.ParserType ([Id], [Name]) VALUES (2, 'FriscoPriceParser')
	SET IDENTITY_INSERT price.ParserType OFF;
END
