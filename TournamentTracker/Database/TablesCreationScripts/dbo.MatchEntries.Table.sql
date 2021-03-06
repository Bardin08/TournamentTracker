USE [Tournaments]
GO
/****** Object:  Table [dbo].[MatchEntries]    Script Date: Вт 19.01.21 23:29:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MatchEntries](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[MatchId] [int] NOT NULL,
	[ParentMatchId] [int] NULL,
	[TeamCompetitingId] [int] NULL,
	[Score] [float] NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[MatchEntries]  WITH CHECK ADD  CONSTRAINT [FK_MatchEntries_Matches] FOREIGN KEY([MatchId])
REFERENCES [dbo].[Matches] ([id])
GO
ALTER TABLE [dbo].[MatchEntries] CHECK CONSTRAINT [FK_MatchEntries_Matches]
GO
ALTER TABLE [dbo].[MatchEntries]  WITH CHECK ADD  CONSTRAINT [FK_MatchEntries_Teams] FOREIGN KEY([TeamCompetitingId])
REFERENCES [dbo].[Teams] ([id])
GO
ALTER TABLE [dbo].[MatchEntries] CHECK CONSTRAINT [FK_MatchEntries_Teams]
GO
