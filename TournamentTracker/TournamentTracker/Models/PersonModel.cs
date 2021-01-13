namespace TournamentTracker.Models
{
    public class PersonModel
    {
        /// <summary>
        /// Represents a team participant`s name.
        /// </summary>
        public string FirstName { get; set; }
        /// <summary>
        /// Represents a team participant`s surname.
        /// </summary>
        public string LastName { get; set; }
        /// <summary>
        /// Represents a team participant`s email address.
        /// </summary>
        public string EmailAddress { get; set; }
        /// <summary>
        /// Represents a team participant`s cellphone number.
        /// </summary>
        public string CellphoneNumber { get; set; }
    }
}