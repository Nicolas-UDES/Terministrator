#region Usings

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#endregion

namespace Terministrator.Terministrator.Entites
{
    /// <summary>
    /// Entity of the user names. Contains all the datas required for a user name of a namable.
    /// </summary>
    class UserName
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserName"/> class.
        /// </summary>
        public UserName()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserName"/> class.
        /// </summary>
        /// <param name="firstName">The first name.</param>
        /// <param name="lastName">The last name.</param>
        /// <param name="username">The username.</param>
        /// <param name="current">if set to <c>true</c> [current].</param>
        /// <param name="changedOn">The changed on.</param>
        /// <param name="ownedBy">The owned by.</param>
        public UserName(string firstName, string lastName, string username, bool current, DateTime changedOn,
            Namable ownedBy)
        {
            FirstName = firstName;
            LastName = lastName;
            Username = username;
            Current = current;
            ChangedOn = changedOn;
            OwnedById = ownedBy.NamableId;
            OwnedBy = ownedBy;
        }

        [Key]
        public int UserNameId { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public bool Current { get; set; }

        [Required]
        public DateTime ChangedOn { get; set; }

        [Required]
        [ForeignKey("OwnedBy")]
        public int OwnedById { get; set; }

        public virtual Namable OwnedBy { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            string retour = FirstName;
            if (!string.IsNullOrEmpty(retour) && !string.IsNullOrEmpty(LastName))
            {
                retour += " ";
            }
            retour += LastName;
            bool parenthesis = !(string.IsNullOrEmpty(retour) || string.IsNullOrEmpty(Username));
            if (parenthesis)
            {
                retour += " (";
            }
            retour += Username;
            if (parenthesis)
            {
                retour += ")";
            }
            return retour;
        }
    }
}