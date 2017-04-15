#region Usings

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#endregion

namespace Terministrator.Terministrator.Entites
{
    class PointsLogReason
    {
        [Key]
        public int PointsLogReasonId { get; set; }

        public string Text { get; set; }
        public virtual List<PointsLog> PointsLogs { get; set; }
    }
}