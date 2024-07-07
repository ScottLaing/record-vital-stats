-------------------------------
-- postgres Member Table
--------------------------------


CREATE TABLE public.Session(
	Id SERIAL PRIMARY KEY,
	MemberId int NOT NULL,
	SessionKey varchar(50) NOT NULL,
	CreateDate timestamp NOT NULL,
	ExpiresDate timestamp NOT NULL,
	Platform varchar(15) NOT NULL,
	IsActive boolean NOT NULL);

ALTER TABLE public.Session ALTER COLUMN CreateDate SET DEFAULT CURRENT_TIMESTAMP;


ALTER TABLE public.Session ALTER COLUMN Platform SET DEFAULT 'rest';


ALTER TABLE public.Member ALTER COLUMN IsActive SET DEFAULT TRUE;


CREATE INDEX idx_Session_MemberId ON public.Session (MemberId);

CREATE INDEX idx_Session_SessionKey ON public.Session (SessionKey);


