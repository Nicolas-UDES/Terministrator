#region Usings

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#endregion

namespace Terministrator.Terministrator.Entites
{
    /// <summary>
    /// Entity of the points log reasons. Contains all the datas required for a reason of a change in the points been logged.
    /// </summary>
    class PointsLogReason
    {
        [Key]
        public int PointsLogReasonId { get; set; }

        public string Text { get; set; }
        public virtual List<PointsLog> PointsLogs { get; set; }
    }
}