#region Usings

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#endregion

namespace Terministrator.Terministrator.Entites
{
    class Domain
    {
        [Key]
        public int DomainId { get; set; }

        public string Name { get; set; }
        public virtual List<Link> Links { get; set; }
        public virtual List<Rules> Rules { get; set; }
    }
}