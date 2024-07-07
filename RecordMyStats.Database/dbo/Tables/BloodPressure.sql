CREATE TABLE [dbo].[BloodPressure](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[MemberId] [int] NOT NULL,
	[RecordingDate] [datetime2](7) NOT NULL,
	[Systolic] [float] NOT NULL,
	[Diastolic] [float] NOT NULL,
	[Units] [varchar](8) NOT NULL,
	[WhenTaken] [varchar](50) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreateDate] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_BloodPressure] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[BloodPressure] ADD  CONSTRAINT [DF_BloodPressure_WhenTaken]  DEFAULT ('') FOR [WhenTaken]
GO

ALTER TABLE [dbo].[BloodPressure] ADD  CONSTRAINT [DF_BloodPressure_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO



