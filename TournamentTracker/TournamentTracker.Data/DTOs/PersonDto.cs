namespace TournamentTracker.Data.DTOs
{
    public class PersonDto
    {
        /// <summary>
        /// Represents person unique identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Represents a team participant`s name.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Represents a team participant`s surname.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Represents person full name. 
        /// </summary>
        public string EmailAddress { get; set; }

        /// <summary>
        /// Represents a team participant`s cellphone number.
        /// </summary>
        public string CellphoneNumber { get; set; }
    }
}