USE [KINTAI]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WorkTime](
	[UserId] [nvarchar](36) NOT NULL,
	[WorkDate] [datetime] NOT NULL,
	[WorkType] [int] NULL,
	[BeginTime] [int] NULL,
	[EndTime] [int] NULL,
	[RestTime] [int] NULL,
	[OfficeTime] [int] NULL,
	[WorkTime] [int] NULL,
	[WorkDetail] [ntext] NULL,
	[CreateTimestamp] [datetime] NULL,
	[CreateUserId] [nvarchar](36) NULL,
	[UpdateTimestamp] [datetime] NULL,
	[UpdateUserId] [nvarchar](36) NULL,
PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[WorkDate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO


