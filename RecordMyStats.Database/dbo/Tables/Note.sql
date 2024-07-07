CREATE TABLE [dbo].[Note](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Description] [varchar](255) NULL,
	[FullText] [varchar](max) NULL,
	[Created] [datetime2](7) NULL,
	[ModBy] [varchar](20) NULL,
	[IsActive] [bit] NOT NULL,
	[Key1] [varchar](100) NULL,
	[Key2] [varchar](100) NULL,
	[Salt] [varchar](40) NULL,
	[IsEncrypted] [bit] NULL,
	[MemberId] [int] NULL,
 CONSTRAINT [PK_Notes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[Note] ADD  CONSTRAINT [DF_Notes_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO

