using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using Kintai.Work;

namespace Kintai
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InputPanel.Visible = false;

            if (Membership.GetUser() != null && User.Identity.IsAuthenticated)
            {
                InputPanel.Visible = true;
                var user = Membership.GetUser();
                string userId = user.ProviderUserKey.ToString();
                WorkTimeEntity entityKey = new WorkTimeEntity();
                entityKey.UserId = userId;
                entityKey.WorkDate = DateTime.Today;

                WorkTimeDao dao = new WorkTimeDao();
                dao.Connection = ThreadConnectionHolder.GetConnection();
                WorkTimeEntity entity = dao.SelectPrimaryKey(entityKey);

                if (entity != null)
                {
                    BeginTime.Text = Utility.MinutesToTimeString(entity.BeginTime);
                    EndTime.Text = Utility.MinutesToTimeString(entity.EndTime);
                }
            }

        }
    }
}
