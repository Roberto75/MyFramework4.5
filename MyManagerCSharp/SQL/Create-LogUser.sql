CREATE TABLE [dbo].[MyLogUser](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[date_added] [datetime] NOT NULL,
	[nota] [varchar](max) NULL,
	[tipo] [varchar](50) NULL,
	[user_id] [int] NOT NULL,
	[ip_address] [varchar](15) NULL,
 CONSTRAINT [PK_MyLogUser] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[MyLogUser]  WITH CHECK ADD  CONSTRAINT [FK_MyLogUser_Utente] FOREIGN KEY([user_id]) REFERENCES [dbo].[Utente] ([user_id])
GO
ALTER TABLE [dbo].[MyLogUser] CHECK CONSTRAINT [FK_MyLogUser_Utente]
GO