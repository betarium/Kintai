USE [Kintai]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[HolidayInfo](
	[HolidayDate] [datetime] NOT NULL,
	[HolidayType] [int] NOT NULL,
	[HolidayName] [nvarchar](20) NULL,
	[CreateTimestamp] [datetime] NULL,
	[CreateUserId] [nvarchar](36) NULL,
	[UpdateTimestamp] [datetime] NULL,
	[UpdateUserId] [nvarchar](36) NULL,
PRIMARY KEY CLUSTERED 
(
	[HolidayDate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


