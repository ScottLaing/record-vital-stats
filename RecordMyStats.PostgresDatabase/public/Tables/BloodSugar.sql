----------------------------------
---- postgres BloodSugar Table
----------------------------------

DROP TABLE IF EXISTS public.BloodSugar;

CREATE TABLE public.BloodSugar (
	Id SERIAL PRIMARY KEY,
	MemberId int NOT NULL,
	Value float NOT NULL,
	Units varchar(8) NOT NULL,
	WhenTaken varchar(20) NOT NULL,
	RecordingDate timestamp NOT NULL,
	Mood int Null,
	Comments varchar(2000) NULL,
	CustomTime varchar(50) NULL,
	IsActive boolean NOT NULL,
	CreateDate timestamp NOT NULL,
	ModifiedDate timestamp NOT NULL);

ALTER TABLE public.BloodSugar ALTER COLUMN Units SET DEFAULT 'mg/dL';

ALTER TABLE public.BloodSugar ALTER COLUMN IsActive SET DEFAULT TRUE;

ALTER TABLE public.BloodSugar ALTER COLUMN CreateDate SET DEFAULT CURRENT_TIMESTAMP;

ALTER TABLE public.BloodSugar ALTER COLUMN RecordingDate SET DEFAULT CURRENT_TIMESTAMP;

ALTER TABLE public.BloodSugar ALTER COLUMN ModifiedDate SET DEFAULT CURRENT_TIMESTAMP;

CREATE INDEX idx_BloodSugar_Email ON public.BloodSugar (Email);




