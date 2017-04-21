#region Usings

using System.ComponentModel.DataAnnotations;

#endregion

namespace Terministrator.Terministrator.Entites
{
    /// <summary>
    /// Entity of the point logs. Contains all the datas required for logging an activity on a user's points, in a channel.
    /// </summary>
    class PointsLog
    {
        [Key]
        public int PointsLogId { get; set; }

        public float Amount { get; set; }
        public string Comment { get; set; }
        public virtual PointsLogReason PointsLogReason { get; set; }

        [Required]
        public virtual UserToChannel To { get; set; }

        public virtual UserToChannel From { get; set; }
    }
}