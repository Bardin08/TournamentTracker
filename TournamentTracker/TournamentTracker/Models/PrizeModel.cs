namespace TournamentTracker.Models
{
    public class PrizeModel
    {
        /// <summary>
        /// Represents the place for which this prize will be given.
        /// </summary>
        public int PlaceNumber { get; set; }
        /// <summary>
        /// Represents the place description.
        /// </summary>
        public string PlaceName { get; set; }
        /// <summary>
        /// Represents a price if it`s a thing.
        /// </summary
        public string PriceName { get; set; }
        /// <summary>
        /// Represents the price amount.
        /// </summary>
        /// <remarks>
        /// Should be used only if a prize is a fixed amount of something.
        /// </remarks>
        public decimal PriceAmount { get; set; }
        /// <summary>
        /// Represents which percent from a full prize fund will receive a person or a team.
        /// </summary>
        /// <remarks>
        /// Should be used only if <see cref="PriceAmount"/> is not used.
        /// </remarks>
        public double PricePercentage { get; set; }
    }
}