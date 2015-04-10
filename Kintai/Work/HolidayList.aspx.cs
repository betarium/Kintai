using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Kintai.Work
{
    public partial class HolidayList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SqlConnection conn;
            conn = ThreadConnectionHolder.GetConnection();
            HolidayInfoDao dao = new HolidayInfoDao(conn);
            HolidayInfoEntity entity = new HolidayInfoEntity();
            List<HolidayInfoEntity> holiday = dao.SelectRange(entity);

        }
    }
}