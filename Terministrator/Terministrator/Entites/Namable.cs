#region Usings

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

#endregion

namespace Terministrator.Terministrator.Entites
{
    abstract class Namable : ApplicationReferencable
    {
        public Namable() { }

        public Namable(Application application, string idForApplication) : base(application, idForApplication) { }

        [Key]
        public int NamableId { get; set; }

        public virtual List<UserName> UserNames { get; set; }

        public string GetFirstName()
        {
            return GetCurrentUserName().FirstName;
        }

        public string GetLastName()
        {
            return GetCurrentUserName().LastName;
        }

        public string GetUsername()
        {
            return GetCurrentUserName().Username;
        }

        public UserName GetCurrentUserName()
        {
            return UserNames.First(x => x.Current);
        }

        public override string ToString()
        {
            return GetCurrentUserName().ToString();
        }
    }
}