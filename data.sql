USE [DataWeb]
GO
/****** Object:  Table [dbo].[Accounts]    Script Date: 1/4/2022 3:11:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Accounts](
	[Username] [varchar](50) NOT NULL,
	[Password] [nvarchar](50) NULL,
	[Fullname] [nvarchar](100) NULL,
	[Permission] [tinyint] NULL,
	[Email] [varchar](50) NULL,
	[Lock] [bit] NULL,
	[Active] [bit] NULL,
 CONSTRAINT [PK_Accounts] PRIMARY KEY CLUSTERED 
(
	[Username] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Menus]    Script Date: 1/4/2022 3:11:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Menus](
	[ID_MN] [bigint] NOT NULL,
	[Parent] [bigint] NULL,
	[Pos] [tinyint] NULL,
	[Lable] [nvarchar](200) NULL,
	[UrlLink] [varchar](200) NULL,
	[OrderKey] [bigint] NULL,
	[UserAdd] [varchar](50) NULL,
	[UserTest] [varchar](50) NULL,
	[Hide] [bit] NULL,
 CONSTRAINT [PK_Menus] PRIMARY KEY CLUSTERED 
(
	[ID_MN] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PageItems]    Script Date: 1/4/2022 3:11:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PageItems](
	[ID_P] [bigint] NOT NULL,
	[ID_MN] [bigint] NULL,
	[Title] [nvarchar](200) NULL,
	[Sumary] [nvarchar](500) NULL,
	[Contents] [ntext] NULL,
	[CreaDate] [smalldatetime] NULL,
	[ModiDate] [smalldatetime] NULL,
	[OrderKey] [bigint] NULL,
	[UserAdd] [varchar](50) NULL,
	[UserTest] [varchar](50) NULL,
	[Hide] [bit] NULL,
	[Image] [varchar](100) NULL,
 CONSTRAINT [PK_PageItems] PRIMARY KEY CLUSTERED 
(
	[ID_P] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[Menus]  WITH CHECK ADD  CONSTRAINT [FK_Menus_Accounts] FOREIGN KEY([UserAdd])
REFERENCES [dbo].[Accounts] ([Username])
GO
ALTER TABLE [dbo].[Menus] CHECK CONSTRAINT [FK_Menus_Accounts]
GO
ALTER TABLE [dbo].[PageItems]  WITH CHECK ADD  CONSTRAINT [FK_PageItems_Menus] FOREIGN KEY([ID_MN])
REFERENCES [dbo].[Menus] ([ID_MN])
GO
ALTER TABLE [dbo].[PageItems] CHECK CONSTRAINT [FK_PageItems_Menus]
GO
