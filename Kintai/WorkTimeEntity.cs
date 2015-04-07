using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kintai
{
    public class WorkTimeEntity
    {
        #region Members
        public string UserId { get; set; }
        public DateTime? WorkDate { get; set; }
        public int? WorkType { get; set; }
        public int? BeginTime { get; set; }
        public int? EndTime { get; set; }
        public int? RestTime { get; set; }
        public int? OfficeTime { get; set; }
        public int? WorkTime { get; set; }
        public string WorkDetail { get; set; }
        public DateTime? CreateTimestamp { get; set; }
        public string CreateUserId { get; set; }
        public DateTime? UpdateTimestamp { get; set; }
        public string UpdateUserId { get; set; }
        #endregion
    }
}
