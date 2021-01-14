USE [Tournaments]
GO
/****** Object:  StoredProcedure [dbo].[spPrizes_GetByTournament]    Script Date: Чт 14.01.21 11:03:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Get all prizes for a given tournament
CREATE PROCEDURE [dbo].[spPrizes_GetByTournament]
	@TournamentId int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT p.*
	FROM dbo.Prizes p
	INNER JOIN dbo.TournamentPrizes t on p.id = t.PrizeId
	WHERE t.TournamentId = @TournamentId;
END
GO
