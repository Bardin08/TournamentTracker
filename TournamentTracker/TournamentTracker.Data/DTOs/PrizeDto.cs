namespace TournamentTracker.Data.DTOs
{
    public class PrizeDto
    {
        /// <summary>
        /// Represents prize unique identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Represents the place for which this prize will be given.
        /// </summary>
        public int PlaceNumber { get; set; }

        /// <summary>
        /// Represents the place description.
        /// </summary>
        public string PlaceName { get; set; }

        /// <summary>
        /// Represents a prize if it`s a thing.
        /// </summary
        public string PrizeName { get; set; }

        /// <summary>
        /// Represents the price amount.
        /// </summary>
        /// <remarks>
        /// Should be used only if a prize is a fixed amount of something.
        /// </remarks>
        public decimal PrizeAmount { get; set; }

        /// <summary>
        /// Represents which percent from a full prize fund will receive a person or a team.
        /// </summary>
        /// <remarks>
        /// Should be used only if <see cref="PriceAmount"/> is not used.
        /// </remarks>
        public double PrizePercentage { get; set; }
    }
}