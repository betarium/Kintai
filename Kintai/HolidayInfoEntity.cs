using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kintai
{
    public class HolidayInfoEntity
    {
        #region Members
        public DateTime? HolidayDate { get; set; }
        public int? HolidayType { get; set; }
        public string HolidayName { get; set; }
        public DateTime? CreateTimestamp { get; set; }
        public string CreateUserId { get; set; }
        public DateTime? UpdateTimestamp { get; set; }
        public string UpdateUserId { get; set; }
        #endregion
    }
}
