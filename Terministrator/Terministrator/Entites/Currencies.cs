#region Usings

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#endregion

namespace Terministrator.Terministrator.Entites
{
    class Currencies
    {
        [Key]
        public int CurrenciesId { get; set; }

        [StringLength(100)]
        [Index("IX_NamePointSystem", 2, IsUnique = true)]
        public string Name { get; set; }

        [Index("IX_ValuePointSystem", 2, IsUnique = true)]
        public float Value { get; set; }

        [Index("IX_NamePointSystem", 1, IsUnique = true)]
        [Index("IX_ValuePointSystem", 1, IsUnique = true)]
        public virtual PointSystem PointSystem { get; set; }
    }
}