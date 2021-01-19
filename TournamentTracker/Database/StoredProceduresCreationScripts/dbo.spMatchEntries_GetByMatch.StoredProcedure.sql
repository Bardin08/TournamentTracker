USE [Tournaments]
GO
/****** Object:  StoredProcedure [dbo].[spMatchEntries_GetByMatch]    Script Date: Вт 19.01.21 23:29:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[spMatchEntries_GetByMatch]
	@MatchId int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT m.*
	FROM dbo.MatchEntries m
	WHERE m.MatchId = @MatchId;

END
GO
