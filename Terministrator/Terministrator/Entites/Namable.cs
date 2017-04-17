#region Usings

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

#endregion

namespace Terministrator.Terministrator.Entites
{
    abstract class Namable : ApplicationReferencable
    {
        public Namable()
        {
        }

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
        public string GetFirstName()
        {
            return GetCurrentUserName().FirstName;
        }

        /// <summary>
        /// Gets the last name of the current <see cref="UserName"/>.
        /// </summary>
        /// <returns>The last name</returns>
        public string GetLastName()
        {
            return GetCurrentUserName().LastName;
        }

        /// <summary>
        /// Gets the username of the current <see cref="UserName"/>.
        /// </summary>
        /// <returns>The username</returns>
        public string GetUsername()
        {
            return GetCurrentUserName().Username;
        }

        /// <summary>
        /// Gets the current <see cref="UserName"/> of the user.
        /// </summary>
        /// <returns>The current UserName</returns>
        public UserName GetCurrentUserName()
        {
            return UserNames.First(x => x.Current);
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return GetCurrentUserName().ToString();
        }
    }
}