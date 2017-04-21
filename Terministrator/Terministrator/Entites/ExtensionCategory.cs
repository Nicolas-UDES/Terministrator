#region Usings

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#endregion

namespace Terministrator.Terministrator.Entites
{
    /// <summary>
    /// Entity of the extension categories. Contains all the datas required for an extension category.
    /// Eg: Image, Text, Video, etc.
    /// </summary>
    class ExtensionCategory
    {
        [Key]
        public int ExtensionCategoryId { get; set; }

        public string Name { get; set; }

        public virtual List<Extension> Extensions { get; set; }
    }
}