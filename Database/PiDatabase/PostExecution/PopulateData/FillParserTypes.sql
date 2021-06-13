
IF NOT EXISTS (SELECT * FROM price.ParserType WHERE [Name] = 'XcomPriceParser')
INSERT INTO price.ParserType ([Name]) VALUES ('XcomPriceParser')
