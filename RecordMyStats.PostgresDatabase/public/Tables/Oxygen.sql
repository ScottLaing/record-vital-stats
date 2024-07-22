----------------------------------
---- postgres Oxygen Table
----------------------------------

DROP TABLE IF EXISTS public.Oxygen;

CREATE TABLE public.Oxygen (
	Id  SERIAL PRIMARY KEY,
	MemberId int NOT NULL,
	OxygenValue int NULL,
	HeartRate int NULL,
	RecordingDate timestamp NOT NULL,
	Mood int Null,
	Comments varchar(2000) NULL,
	WhenTaken varchar(200) NULL,
	CreateDate timestamp NOT NULL,
	ModifiedDate timestamp NOT NULL,
	IsActive boolean NOT NULL
);

ALTER TABLE public.Oxygen ALTER COLUMN RecordingDate SET DEFAULT CURRENT_TIMESTAMP;

ALTER TABLE public.Oxygen ALTER COLUMN IsActive SET DEFAULT TRUE;

ALTER TABLE public.Oxygen ALTER COLUMN CreateDate SET DEFAULT CURRENT_TIMESTAMP;

ALTER TABLE public.Oxygen ALTER COLUMN ModifiedDate SET DEFAULT CURRENT_TIMESTAMP;

CREATE INDEX Idx_Oxygen_MemberId ON public.Oxygen (MemberId);


