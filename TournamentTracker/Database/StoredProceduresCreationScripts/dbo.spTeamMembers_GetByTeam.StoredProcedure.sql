USE [Tournaments]
GO
/****** Object:  StoredProcedure [dbo].[spTeamMembers_GetByTeam]    Script Date: Чт 14.01.21 11:03:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[spTeamMembers_GetByTeam]
	@TeamId int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT p.*
	FROM dbo.People p
	INNER JOIN dbo.TeamMembers tm on p.id = tm.PersonId
	WHERE tm.TeamId = @TeamId;

END
GO
