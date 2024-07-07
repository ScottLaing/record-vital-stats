﻿----------------------------------
---- postgres Note Table
----------------------------------
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
	MemberId int NULL);

ALTER TABLE public.Note ALTER COLUMN IsActive SET DEFAULT TRUE;

ALTER TABLE public.Note ALTER COLUMN Created SET DEFAULT CURRENT_TIMESTAMP;

CREATE INDEX Idx_Note_MemberId ON public.Note (MemberId);

