
IF NOT EXISTS (SELECT * FROM pi.Settings WHERE [Name] = '2MinersEth')
INSERT INTO pi.Settings ([Name], [Value], [Description]) VALUES ('2MinersEth', 'https://eth.2miners.com/api', '2Miners API endpoint for ETH')
