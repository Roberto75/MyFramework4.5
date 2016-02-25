CREATE TABLE [dbo].[Ticket](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[OWNER_ID] [int] NULL,
	[TARGET_ID] [nvarchar](50) NULL,
	[TITOLO] [nvarchar](max) NULL,
	[TICKET_TYPE_ID] [nvarchar](50)  NULL,
	[DATE_LAST_MODIFIED] [datetime] NULL,
	[TICKET_STATUS_ID] [nvarchar](50) NOT NULL,
	date_added datetime NOT NULL,
	REFERENCE_TYPE_ID [nvarchar](50) NULL,
	REFERENCE_ID [int] NULL,
	REFERENCE_SOURCE_ID [int] NULL,
	REFERENCE_SOURCE [nvarchar](50) NULL,
 CONSTRAINT [PK_Ticket] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [dbo].[Ticket]  WITH CHECK ADD  CONSTRAINT [FK_Ticket_Utente] FOREIGN KEY([OWNER_ID]) REFERENCES [dbo].[Utente] ([user_id])
GO
ALTER TABLE [dbo].[Ticket] CHECK CONSTRAINT [FK_Ticket_Utente]
GO
--ALTER TABLE [dbo].[Ticket]  WITH CHECK ADD  CONSTRAINT [FK_Ticket_Gruppo] FOREIGN KEY([OWNER_GROUP_ID]) REFERENCES [dbo].[Gruppo] ([gruppo_id])
--GO
--ALTER TABLE [dbo].[Ticket] CHECK CONSTRAINT [FK_Ticket_Gruppo]
--GO





CREATE TABLE [dbo].[Ticket_Post](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[isFirstPost] [bit] NOT NULL,
	[USER_ID] [int] NOT NULL,
	[TICKET_ID] [int] NOT NULL,
	[NOTE] [nvarchar](max) NOT NULL,
	[date_added] [datetime] NOT NULL,
	[FK_POST_ID] [int]  NULL
 CONSTRAINT [PK_Ticket_Post] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [dbo].[Ticket_Post]  WITH CHECK ADD  CONSTRAINT [FK_Ticket_Post_Ticket] FOREIGN KEY([TICKET_ID]) REFERENCES [dbo].[Ticket] ([id])
GO
ALTER TABLE [dbo].[Ticket_Post] CHECK CONSTRAINT [FK_Ticket_Post_Ticket]
GO

ALTER TABLE [dbo].[Ticket_Post]  WITH CHECK ADD  CONSTRAINT [FK_Ticket_Post_Utente] FOREIGN KEY(USER_ID) REFERENCES [dbo].[Utente] ([user_id])
GO
ALTER TABLE [dbo].[Ticket_Post] CHECK CONSTRAINT [FK_Ticket_Post_Utente]
GO

ALTER TABLE [dbo].[Ticket_Post]  WITH CHECK ADD  CONSTRAINT [FK_Ticket_Post_Post] FOREIGN KEY(FK_POST_ID) REFERENCES [dbo].[Ticket_Post] ([id])
GO
ALTER TABLE [dbo].[Ticket_Post] CHECK CONSTRAINT [FK_Ticket_Post_Post]
GO








CREATE TABLE dbo.Ticket_Attachment
	(
	id int NOT NULL IDENTITY (1, 1),
	user_id int NOT NULL,
	ticket_id int NOT NULL,
	note nvarchar(MAX) NULL,
	date_added datetime NOT NULL,
	file_name nvarchar(MAX) NOT NULL
	)  ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE dbo.Ticket_Attachment ADD CONSTRAINT	PK_Ticket_Attachment PRIMARY KEY CLUSTERED 	(	id	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].Ticket_Attachment  WITH CHECK ADD  CONSTRAINT [FK_Ticket_Attachment_Ticket] FOREIGN KEY(ticket_id) REFERENCES [dbo].[Ticket] ([id])
GO
ALTER TABLE [dbo].Ticket_Attachment CHECK CONSTRAINT [FK_Ticket_Attachment_Ticket]
GO
ALTER TABLE [dbo].Ticket_Attachment  WITH CHECK ADD  CONSTRAINT [FK_Ticket_Attachment_Utente] FOREIGN KEY(USER_ID) REFERENCES [dbo].[Utente] ([user_id])
GO
ALTER TABLE [dbo].Ticket_Attachment CHECK CONSTRAINT [FK_Ticket_Attachment_Utente]