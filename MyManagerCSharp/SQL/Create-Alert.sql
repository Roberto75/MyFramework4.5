CREATE TABLE [dbo].[MyAlert](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[date_added] [datetime] NOT NULL,
	[nome] [varchar](50) NOT NULL,
	[descrizione] [varchar](max) NOT NULL,
	[is_enabled] bit NOT NULL,
	CONSTRAINT [PK_MyAlert] PRIMARY KEY CLUSTERED 
	(
		[id] ASC
	)WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
)
GO


CREATE TABLE [dbo].[MyAlert_Utente](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[USER_ID] [int] NOT NULL,
	[ALERT_ID] [int] NOT NULL,
	[date_added] [datetime] NOT NULL,
	CONSTRAINT [PK_MyAlert_Utente] PRIMARY KEY CLUSTERED 
	(
		[id] ASC
	)WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) 
GO

ALTER TABLE [dbo].[MyAlert_Utente]  WITH CHECK ADD  CONSTRAINT [FK_MyAlert_Utente_MyAlert] FOREIGN KEY([ALERT_ID]) REFERENCES [dbo].[MyAlert] ([id])
GO
ALTER TABLE [dbo].[MyAlert_Utente] CHECK CONSTRAINT [FK_MyAlert_Utente_MyAlert]
GO
ALTER TABLE [dbo].[MyAlert_Utente]  WITH CHECK ADD  CONSTRAINT [FK_MyAlert_Utente_Utente] FOREIGN KEY(USER_ID) REFERENCES [dbo].[Utente] ([user_id])
GO
ALTER TABLE [dbo].[MyAlert_Utente] CHECK CONSTRAINT [FK_MyAlert_Utente_Utente]
GO


INSERT INTO MyAlert ( is_enabled, date_added, nome , descrizione) VALUES ( 1, GetDate(), 'VULN_DEEPSIGHT' ,'Vulnerabilità pubblicate da Deep Sight' );
GO
INSERT INTO MyAlert ( is_enabled, date_added, nome , descrizione) VALUES ( 1, GetDate(), 'VULN_NVD' ,'Vulnerabilità pubblicate da NVD' );
GO
INSERT INTO MyAlert ( is_enabled, date_added, nome , descrizione) VALUES ( 1, GetDate(), 'VULN_CERT' ,'Vulnerabilità pubblicate dal CERT' );
GO
INSERT INTO MyAlert ( is_enabled, date_added, nome , descrizione) VALUES ( 1, GetDate(), 'MALWARE_CERT' ,'Malicious code pubblicati dal CERT' );
GO
INSERT INTO MyAlert ( is_enabled, date_added, nome , descrizione) VALUES ( 1, GetDate(), 'MALWARE_DEEPSIGHT' ,'Malicious code pubblicati da Deep Sight' );
GO

