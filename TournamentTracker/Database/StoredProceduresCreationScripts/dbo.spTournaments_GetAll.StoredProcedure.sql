USE [Tournaments]
GO
/****** Object:  StoredProcedure [dbo].[spTournaments_GetAll]    Script Date: Вт 19.01.21 23:29:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[spTournaments_GetAll]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT *
	FROM dbo.Tournaments;

END
GO
