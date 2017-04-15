#region Usings

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#endregion

namespace Terministrator.Terministrator.Entites
{
    class ExtensionCategory
    {
        [Key]
        public int ExtensionCategoryId { get; set; }

        public string Name { get; set; }

        public virtual List<Extension> Extensions { get; set; }
    }
}