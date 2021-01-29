namespace TournamentTracker.Domain.Models
{
    public class PrizeModel
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

        /// <summary>
        /// Creates a prize model.
        /// </summary>
        public PrizeModel()
        {
        }
        /// <summary>
        /// Creates a prize model.
        /// </summary>
        /// <param name="placeNumber">Represents the place for which this prize will be given.</param>
        /// <param name="placeName">Represents the place description.</param>
        /// <param name="prizeName">Represents a prize if it`s a thing.</param>
        /// <param name="prizeAmount">Represents which percent from a full prize fund will receive a person or a team.</param>
        public PrizeModel(int placeNumber, string placeName, string prizeName, decimal prizeAmount)
        {
            if (string.IsNullOrEmpty(placeName))
            {
                throw new System.ArgumentException($"\"{nameof(placeName)}\" can`t be null or empty", nameof(placeName));
            }

            if (string.IsNullOrEmpty(prizeName))
            {
                throw new System.ArgumentException($"\"{nameof(prizeName)}\" can`t be null or empty", nameof(prizeName));
            }

            if (placeNumber < 1)
            {
                throw new System.ArgumentException($"\"{nameof(placeNumber)}\" can`t be less then 1", nameof(placeNumber));
            }

            PlaceNumber = placeNumber;
            PlaceName = placeName;
            PrizeName = prizeName;
            PrizeAmount = prizeAmount;
            PrizePercentage = 0;
        }
        /// <summary>
        /// Creates a prize model.
        /// </summary>
        /// <param name="placeNumber">Represents the place for which this prize will be given.</param>
        /// <param name="placeName">Represents the place description.</param>
        /// <param name="prizeName">Represents a prize if it`s a thing.</param>
        /// <param name="pricePercentage">Represents which percent from a full prize fund will receive a person or a team.</param>
        public PrizeModel(int placeNumber, string placeName, string prizeName, double pricePercentage)
        {
            if (string.IsNullOrEmpty(placeName))
            {
                throw new System.ArgumentException($"\"{nameof(placeName)}\" can`t be null or empty", nameof(placeName));
            }

            if (string.IsNullOrEmpty(prizeName))
            {
                throw new System.ArgumentException($"\"{nameof(prizeName)}\" can`t be null or empty", nameof(prizeName));
            }

            if (placeNumber < 1)
            {
                throw new System.ArgumentException($"\"{nameof(placeNumber)}\" can`t be less then 1", nameof(placeNumber));
            }

            PlaceNumber = placeNumber;
            PlaceName = placeName;
            PrizeName = prizeName;
            PrizePercentage = pricePercentage;
            PrizeAmount = 0;
        }
    }
}
