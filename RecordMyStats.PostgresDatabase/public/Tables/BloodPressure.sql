﻿----------------------------------
---- postgres BloodPressure Table
----------------------------------

DROP TABLE IF EXISTS public.BloodPressure;

CREATE TABLE public.BloodPressure (
	Id  SERIAL PRIMARY KEY,
	MemberId int NOT NULL,
	Systolic int NULL,
	Diastolic int NULL,
	Units varchar(8) NOT NULL,
	HeartRate int NULL,
	RecordingDate timestamp NOT NULL,
	Mood int Null,
	Comments varchar(2000) NULL,
	WhenTaken varchar(200) NULL,
	CreateDate timestamp NOT NULL,
	ModifiedDate timestamp NOT NULL,
	IsActive boolean NOT NULL
);

ALTER TABLE public.BloodPressure ALTER COLUMN Units SET DEFAULT 'mmHg';

ALTER TABLE public.BloodPressure ALTER COLUMN RecordingDate SET DEFAULT CURRENT_TIMESTAMP;

ALTER TABLE public.BloodPressure ALTER COLUMN IsActive SET DEFAULT TRUE;

ALTER TABLE public.BloodPressure ALTER COLUMN CreateDate SET DEFAULT CURRENT_TIMESTAMP;

ALTER TABLE public.BloodPressure ALTER COLUMN ModifiedDate SET DEFAULT CURRENT_TIMESTAMP;

CREATE INDEX Idx_BloodPressure_MemberId ON public.BloodPressure (MemberId);


