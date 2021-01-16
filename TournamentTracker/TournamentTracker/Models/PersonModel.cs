namespace TournamentTracker.Models
{
    public class PersonModel
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
        public string FullName => $"{FirstName} {LastName}";
        /// <summary>
        /// Represents a team participant`s email address.
        /// </summary>
        public string EmailAddress { get; set; }
        /// <summary>
        /// Represents a team participant`s cellphone number.
        /// </summary>
        public string CellphoneNumber { get; set; }


        /// <summary>
        /// Constructs a person model.
        /// </summary>
        public PersonModel()
        {
        }

        /// <summary>
        /// Constructs a person model.
        /// </summary>
        /// <param name="firstName"> Person`s first name </param>
        /// <param name="lastName"> Person`s last name </param>
        /// <param name="emailAddress"> Person`s email address </param>
        /// <param name="cellphoneNumber"> Person`s cellphoneNumber </param>
        public PersonModel(string firstName, string lastName, string emailAddress, string cellphoneNumber)
        {
            FirstName = firstName ?? throw new System.ArgumentNullException(nameof(firstName));
            LastName = lastName ?? throw new System.ArgumentNullException(nameof(lastName));
            EmailAddress = emailAddress ?? throw new System.ArgumentNullException(nameof(emailAddress));
            CellphoneNumber = cellphoneNumber ?? throw new System.ArgumentNullException(nameof(cellphoneNumber));
        }
    }
}
