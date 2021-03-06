USE [Tournaments]
GO
/****** Object:  StoredProcedure [dbo].[spTeams_GetByTournament]    Script Date: Вт 19.01.21 23:29:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[spTeams_GetByTournament]
	@TournamentId int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT t.*
	FROM dbo.Teams t
	INNER JOIN dbo.TournamentEntries te on te.TeamId = t.id
	WHERE te.TournamentId = @TournamentId;
END
GO
