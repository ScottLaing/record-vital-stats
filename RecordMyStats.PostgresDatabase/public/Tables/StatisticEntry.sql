----------------------------------
---- postgres StatisticEntry Table
----------------------------------

-- moving towards making this table obsolete in latest thinking

DROP TABLE IF EXISTS public.StatisticEntry;

CREATE TABLE public.StatisticEntry (
	Id  SERIAL PRIMARY KEY,
	MemberId int NOT NULL,
	CreateDate timestamp NOT NULL,
	BloodSugar float NULL,
	Weight float NULL,
	BPSystolic int NULL,
	BPDiastolic int NULL,
	HeartRate int NULL,
	WeightUnits varchar(5) NOT NULL,
	BSUnits varchar(8) NOT NULL);

ALTER TABLE public.StatisticEntry ALTER COLUMN CreateDate SET DEFAULT CURRENT_TIMESTAMP;

ALTER TABLE public.StatisticEntry ALTER COLUMN WeightUnits SET DEFAULT 'lb';

ALTER TABLE public.StatisticEntry ALTER COLUMN BSUnits SET DEFAULT 'system';

CREATE INDEX Idx_StatisticEntry_MemberId ON public.StatisticEntry (MemberId);




