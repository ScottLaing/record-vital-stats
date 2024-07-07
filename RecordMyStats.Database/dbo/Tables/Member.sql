CREATE TABLE [dbo].[Member](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [varchar](50) NULL,
	[LastName] [varchar](50) NULL,
	[MiddleName] [varchar](50) NULL,
	[IsActive] [bit] NOT NULL,
	[Email] [varchar](255) NOT NULL,
	[DateOfBirth] [date] NULL,
	[Sex] [char](1) NULL,
	[Password] [varchar](100) NULL,
	[Zip] [varchar](8) NULL,
	[Country] [varchar](50) NULL,
	[CreateDate] [datetime2](7) NULL,
	[ModDate] [datetime2](7) NULL,
	[ModBy] [varchar](15) NULL,
 CONSTRAINT [PK_Member] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Member] ADD  CONSTRAINT [DF_Member_CreateDate]  DEFAULT (sysdatetime()) FOR [CreateDate]
GO

ALTER TABLE [dbo].[Member] ADD  CONSTRAINT [DF_Member_ModDate]  DEFAULT (sysdatetime()) FOR [ModDate]
GO

ALTER TABLE [dbo].[Member] ADD  CONSTRAINT [DF_Member_ModBy]  DEFAULT ('system') FOR [ModBy]
GO

/****** Object:  Index [Idx_Member_Email]    Script Date: 1/21/2023 3:59:54 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [Idx_Member_Email] ON [dbo].[Member]
(
	[Email] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

