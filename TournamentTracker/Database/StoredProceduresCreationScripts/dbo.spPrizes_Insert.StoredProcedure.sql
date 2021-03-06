USE [Tournaments]
GO
/****** Object:  StoredProcedure [dbo].[spPrizes_Insert]    Script Date: Вт 19.01.21 23:29:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spPrizes_Insert]
	@PlaceNumber int,
	@PlaceName nvarchar(50),
	@PrizeName nvarchar(100),
	@PrizeAmount money,
	@PrizePercentage float,
	@id int = 0 output

AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO dbo.Prizes (PlaceNumber, PlaceName, PrizeName, PrizeAmount, PrizePercentage) 
	VALUES (@PlaceNumber, @PlaceName, @PrizeName, @PrizeAmount, @PrizePercentage)

	SELECT @id = SCOPE_IDENTITY();
END
GO
