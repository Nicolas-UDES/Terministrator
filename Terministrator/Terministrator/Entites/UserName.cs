#region Usings

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#endregion

namespace Terministrator.Terministrator.Entites
{
    class UserName
    {
        public UserName()
        {
        }

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