#region Usings

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

#endregion

namespace Terministrator.Terministrator.Entites
{
    /// <summary>
    /// Says that an entity can hold a username. This let us keep track of the history of usernames if they're changed.
    /// </summary>
    /// <seealso cref="ApplicationReferencable" />
    abstract class Namable : ApplicationReferencable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Namable"/> class.
        /// </summary>
        public Namable()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Namable"/> class.
        /// </summary>
        /// <param name="application">The application.</param>
        /// <param name="idForApplication">The identifier for application.</param>
        public Namable(Application application, string idForApplication) : base(application, idForApplication)
        {
        }

        [Key]
        public int NamableId { get; set; }

        public virtual List<UserName> UserNames { get; set; }

        /// <summary>
        /// Gets the first name of the current <see cref="UserName"/>.
        /// </summary>
        /// <returns>The first name</returns>
        [NotMapped]
        public string FirstName => CurrentUserName.FirstName;

        /// <summary>
        /// Gets the last name of the current <see cref="UserName"/>.
        /// </summary>
        /// <returns>The last name</returns>
        [NotMapped]
        public string LastName => CurrentUserName.LastName;

        /// <summary>
        /// Gets the username of the current <see cref="UserName"/>.
        /// </summary>
        /// <returns>The username</returns>
        [NotMapped]
        public string Username => CurrentUserName.Username;

        /// <summary>
        /// Gets the current <see cref="UserName"/> of the user.
        /// </summary>
        /// <returns>The current UserName</returns>
        [NotMapped]
        public UserName CurrentUserName => UserNames.First(x => x.Current);

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return CurrentUserName.ToString();
        }
    }
}