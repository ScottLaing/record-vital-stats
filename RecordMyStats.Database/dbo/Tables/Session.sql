CREATE TABLE [dbo].[Session](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[MemberId] [int] NOT NULL,
	[SessionKey] [varchar](50) NOT NULL,
	[CreateDate] [datetime2](7) NOT NULL,
	[ExpiresDate] [datetime2](7) NOT NULL,
	[Platform] [varchar](15) NOT NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_Session] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Session] ADD  CONSTRAINT [DF_Session_CreateDate]  DEFAULT (sysdatetime()) FOR [CreateDate]
GO

ALTER TABLE [dbo].[Session] ADD  CONSTRAINT [DF_Session_Platform]  DEFAULT ('rest') FOR [Platform]
GO

ALTER TABLE [dbo].[Session] ADD  CONSTRAINT [DF_Session_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO

/****** Object:  Index [Idx_Session_MemberId]    Script Date: 1/21/2023 4:03:48 PM ******/
CREATE NONCLUSTERED INDEX [Idx_Session_MemberId] ON [dbo].[Session]
(
	[MemberId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

/****** Object:  Index [Idx_Session_SessionKey]    Script Date: 1/21/2023 4:04:11 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [Idx_Session_SessionKey] ON [dbo].[Session]
(
	[SessionKey] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO


