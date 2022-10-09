
select * from pi.price.Parser

update price.Parser SET ActiveTo = '2021-12-01'

select * from price.ParserType

insert into price.ParserType values ('CastoramaPriceParser', NULL)

insert into price.Parser values
(
	'https://www.castorama.pl/deska-szalunkowa-blooma-impregnowana-22-x-100-x-3000-mm-id-1094996.html',
	3,
	NULL,
	1,
	GETDATE(),
	GETDATE(),
	'2021-01-01',
	'9999-01-01'
)

select * from price.PriceSeries