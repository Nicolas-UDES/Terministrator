#region Usings

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#endregion

namespace Terministrator.Terministrator.Entites
{
    /// <summary>
    /// Entity of the extensions. Contains all the datas required for a file extension.
    /// </summary>
    class Extension
    {
        [Key]
        public int ExtensionId { get; set; }

        public string Name { get; set; }
        public virtual ExtensionCategory ExtensionCategory { get; set; }
        public virtual List<File> Files { get; set; }
        public virtual List<Rules> Rules { get; set; }
    }
}