using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Kintai.Work
{
    public partial class HolidayRegister : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void SaveButton_Click(object sender, EventArgs e)
        {
            SaveDate();
        }

        protected void SaveDate()
        {
            HolidayInfoEntity entity = new HolidayInfoEntity();
            entity.Holiday = HolidayCalendar.SelectedDate;
            HolidayInfoDao dao = new HolidayInfoDao();

            dao.Connection = ThreadConnectionHolder.GetConnection();
            HolidayInfoEntity oldentity = dao.SelectPrimaryKey(entity);
            if (oldentity != null)
            {
                entity = oldentity;
            }
            
            entity.HolidayName = HolidayName.Text;

            if (oldentity == null)
            {
                dao.Insert(entity);
            }
            else
            {
                dao.UpdatePrimaryKey(entity);
            }

            HolidayName.Text = "";
            SuccessMessage.Text = "更新完了しました。";
            SuccessMessage.Visible = true;
        }

        protected void Calendar_SelectionChanged(object sender, EventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine(HolidayCalendar.SelectedDate);
            //HolidayInfoEntity entity = new HolidayInfoEntity();
            //entity.Holiday = HolidayCalendar.SelectedDate;
        }


    }
}