CREATE TABLE [dbo].[StatisticEntry](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[MemberId] [int] NOT NULL,
	[CreateDate] [datetime2](7) NOT NULL,
	[BloodSugar] [float] NULL,
	[Weight] [float] NULL,
	[BPSystolic] [int] NULL,
	[BPDiastolic] [int] NULL,
	[HeartRate] [int] NULL,
	[WeightUnits] [varchar](5) NOT NULL,
	[BSUnits] [varchar](8) NOT NULL,
 CONSTRAINT [PK_StatisticEntry] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[StatisticEntry] ADD  CONSTRAINT [DF_StatisticEntry_CreateDate]  DEFAULT (sysdatetime()) FOR [CreateDate]
GO

ALTER TABLE [dbo].[StatisticEntry] ADD  CONSTRAINT [DF_StatisticEntry_WeightUnits]  DEFAULT ('lb') FOR [WeightUnits]
GO

ALTER TABLE [dbo].[StatisticEntry] ADD  CONSTRAINT [DF_StatisticEntry_BSUnits]  DEFAULT ('mg/dL') FOR [BSUnits]
GO

/****** Object:  Index [Idx_StatisticsEntry_MemberId]    Script Date: 1/21/2023 4:05:50 PM ******/
CREATE NONCLUSTERED INDEX [Idx_StatisticsEntry_MemberId] ON [dbo].[StatisticEntry]
(
	[MemberId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

