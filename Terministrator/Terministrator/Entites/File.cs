#region Usings

using System.ComponentModel.DataAnnotations;

#endregion

namespace Terministrator.Terministrator.Entites
{
    class File
    {
        [Key]
        public int FileId { get; set; }

        public string FileName { get; set; }
        public virtual Message Message { get; set; }
    }
}