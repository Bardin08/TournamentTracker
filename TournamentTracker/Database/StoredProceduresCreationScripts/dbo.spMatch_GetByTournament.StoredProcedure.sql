USE [Tournaments]
GO
/****** Object:  StoredProcedure [dbo].[spMatch_GetByTournament]    Script Date: Вт 19.01.21 23:29:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[spMatch_GetByTournament]
	@TournamentId int

AS
BEGIN
	SET NOCOUNT ON;

	SELECT m.*
	FROM dbo.Matches m
	WHERE m.id = (
		SELECT me.MatchId
		FROM dbo.MatchEntries me
		WHERE me.TeamCompetitingId = (
			SELECT t.id
			FROM dbo.Teams t
			INNER JOIN dbo.TournamentEntries te on  t.id = te.TeamId
			WHERE te.TournamentId = @TournamentId 
		) 
	)

END
GO
