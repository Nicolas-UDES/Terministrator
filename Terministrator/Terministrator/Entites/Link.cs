#region Usings

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#endregion

namespace Terministrator.Terministrator.Entites
{
    class Link
    {
        [Key]
        public int LinkId { get; set; }

        public string URL { get; set; }
        public virtual Message Message { get; set; }
        public virtual List<Domain> Domains { get; set; }
    }
}