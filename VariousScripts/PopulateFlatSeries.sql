
DECLARE @day datetime = '2021-02-02';
with randowvalues AS (
	select 1 id, @day [day], 
	CAST(RAND(CHECKSUM(NEWID()))*100 as real) +7000 avgPrivePerMeter, 
	CAST(RAND(CHECKSUM(NEWID()))*1000 as real) +370000 avgPrive, 
	NULL category, 
	CAST(RAND(CHECKSUM(NEWID()))*10 as int) +600 amount
	--,NULL, NULL, NULL, NULL, NULL

    union  ALL

    select id + 1, dateadd(D, 1, [day]), 
	CAST(RAND(CHECKSUM(NEWID()))*100 as real) + 7000 + id avgPrivePerMeter, 
	CAST(RAND(CHECKSUM(NEWID()))*1000 as real) + 370000 + id avgPrive, 
	NULL category, 
	CAST(RAND(CHECKSUM(NEWID()))*10 as int) + 600 + id amount
	--,NULL, NULL, NULL, NULL, NULL
    from randowvalues
    where id < 100
)
INSERT INTO otodom.FlatSeries 
SELECT rw.day, rw.avgPrivePerMeter, rw.avgPrive, NULL, rw.amount, NULL, NULL, NULL, NULL, NULL
from randowvalues rw
OPTION(MAXRECURSION 0)

