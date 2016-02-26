CREATE TABLE [dbo].[MyLogEntity](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[date_added] [datetime] NOT NULL,
	[user_name] [varchar](150)  NULL,
	[entity_id] [int]  NULL,
	[entity_name] [varchar](150)  NULL,
	[entity_values] [varchar](max) NULL,
 CONSTRAINT [PK_MyLogEntity] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO