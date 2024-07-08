----------------------------------
---- postgres Member Table
----------------------------------

DROP TABLE IF EXISTS public.Member;

CREATE TABLE public.Member (
	Id SERIAL PRIMARY KEY,
	FirstName varchar(50) NULL,
	LastName varchar(50) NULL,
	MiddleName varchar(50) NULL,
	IsActive boolean NOT NULL,
	Email varchar(255) NOT NULL,
	DateOfBirth date NULL,
	Sex char(1) NULL,
	Password varchar(100) NULL,
	Zip varchar(8) NULL,
	Country varchar(50) NULL,
	CreateDate timestamp NULL,
	ModBy varchar(15) NULL,
	ModifiedDate timestamp NOT NULL);

ALTER TABLE public.Member ALTER COLUMN CreateDate SET DEFAULT CURRENT_TIMESTAMP;

ALTER TABLE public.Member ALTER COLUMN ModBy SET DEFAULT 'system';

ALTER TABLE public.Member ALTER COLUMN ModifiedDate SET DEFAULT CURRENT_TIMESTAMP;

CREATE INDEX Idx_Member_Email ON public.Member (Email);


