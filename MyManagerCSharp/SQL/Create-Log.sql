CREATE TABLE [dbo].[MyLog](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[date_added] [datetime] NOT NULL,
	[session_id] [varchar](50)  NULL,
	[reference] [varchar](50)  NULL,
	[my_level] [varchar](50)  NULL,
	[my_note] [varchar](max) NULL,
	[my_source] [varchar](50) NULL,
 CONSTRAINT [PK_MyLog] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO