CREATE TABLE [dbo].[BloodSugar](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[MemberId] [int] NOT NULL,
	[Value] [float] NOT NULL,
	[Units] [varchar](8) NOT NULL,
	[WhenTaken] [varchar](20) NOT NULL,
	[Notes] [varchar](500) NULL,
	[MoodLevel] [int] NULL,
	[CustomTime] [varchar](50) NULL,
	[IsActive] [bit] NOT NULL,
	[CreateDate] [datetime2](7) NOT NULL,
	[RecordingDate] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_BloodSugar] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[BloodSugar] ADD  CONSTRAINT [DF_BloodSugar_Units]  DEFAULT ('mg/dL') FOR [Units]
GO

ALTER TABLE [dbo].[BloodSugar] ADD  CONSTRAINT [DF_BloodSugar_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO

ALTER TABLE [dbo].[BloodSugar] ADD  CONSTRAINT [DF_BloodSugar_CreateDate]  DEFAULT (sysdatetime()) FOR [CreateDate]
GO

