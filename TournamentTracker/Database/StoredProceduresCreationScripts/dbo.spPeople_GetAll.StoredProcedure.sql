USE [Tournaments]
GO
/****** Object:  StoredProcedure [dbo].[spPeople_GetAll]    Script Date: Чт 14.01.21 11:03:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Get all people
CREATE PROCEDURE [dbo].[spPeople_GetAll]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT *
	FROM dbo.People;
END
GO
