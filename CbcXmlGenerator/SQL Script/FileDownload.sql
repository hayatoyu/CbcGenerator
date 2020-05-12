USE [CbcXmlGenerator]
GO

ALTER TABLE [dbo].[FileDownload] DROP CONSTRAINT [DF_FileDownload_CreateDate]
GO

/****** Object:  Table [dbo].[FileDownload]    Script Date: 2020/5/12 ¤W¤È 10:03:06 ******/
DROP TABLE [dbo].[FileDownload]
GO

/****** Object:  Table [dbo].[FileDownload]    Script Date: 2020/5/12 ¤W¤È 10:03:06 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[FileDownload](
	[Id] [nvarchar](50) NOT NULL,
	[FileName] [nvarchar](100) NOT NULL,
	[FileStream] [varbinary](max) NULL,
	[CreateDate] [datetime] NOT NULL,
 CONSTRAINT [PK_FileDownload] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[FileDownload] ADD  CONSTRAINT [DF_FileDownload_CreateDate]  DEFAULT (getdate()) FOR [CreateDate]
GO


