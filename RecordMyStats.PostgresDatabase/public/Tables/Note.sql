----------------------------------
---- postgres Note Table
----------------------------------

DROP TABLE IF EXISTS public.Note;

CREATE TABLE public.Note (
	Id SERIAL PRIMARY KEY,
	Description varchar(255) NULL,
	FullText varchar(8000) NULL,
	Created timestamp NULL,
	ModBy varchar(20) NULL,
	IsActive boolean NOT NULL,
	Key1 varchar(100) NULL,
	Key2 varchar(100) NULL,
	Salt varchar(40) NULL,
	IsEncrypted boolean NULL,
	MemberId int NULL,
	ModifiedDate timestamp NOT NULL);

ALTER TABLE public.Note ALTER COLUMN IsActive SET DEFAULT TRUE;

ALTER TABLE public.Note ALTER COLUMN Created SET DEFAULT CURRENT_TIMESTAMP;

ALTER TABLE public.Note ALTER COLUMN ModifiedDate SET DEFAULT CURRENT_TIMESTAMP;

CREATE INDEX Idx_Note_MemberId ON public.Note (MemberId);

