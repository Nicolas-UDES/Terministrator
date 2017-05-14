#region Usings

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#endregion

namespace Terministrator.Terministrator.Entites
{
    /// <summary>
    /// Entity of the links. Contains all the datas required for a link sent by a user.
    /// </summary>
    class Link
    {
        [Key]
        public int LinkId { get; set; }

        public string URL { get; set; }
        public virtual Text Text { get; set; }
        public virtual List<Domain> Domains { get; set; }
    }
}