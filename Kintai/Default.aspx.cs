using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using Kintai.Work;
using System.Web.Configuration;

namespace Kintai
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InputPanel.Visible = false;
            TargetDate.Text = DateTime.Today.ToString("yyyy年MM月dd日(ddd)");

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

            MessagePanel.Visible = false;
            string topMessage = WebConfigurationManager.AppSettings["TopMessage"];
            if (!string.IsNullOrEmpty(topMessage))
            {
                TopMessage.Text = topMessage;
                MessagePanel.Visible = true;
            }

        }

        protected void UpdateWorkData(bool begin, bool end)
        {
            WorkTimeDao dao = new WorkTimeDao();
            dao.Connection = ThreadConnectionHolder.GetConnection();

            var user = Membership.GetUser();
            string userId = user.ProviderUserKey.ToString();
            WorkTimeEntity entityKey = new WorkTimeEntity();
            entityKey.UserId = userId;
            entityKey.WorkDate = DateTime.Today;

            WorkTimeEntity oldentity = dao.SelectPrimaryKey(entityKey);
            WorkTimeEntity entity = new WorkTimeEntity();
            if (oldentity != null)
            {
                entity = oldentity;
            }

            entity.UserId = entityKey.UserId;
            entity.WorkDate = entityKey.WorkDate;
            entity.WorkType = (int)Utility.WorkType.Normal;
            if (begin)
            {
                entity.BeginTime = Utility.TimeStringToMinutes(DateTime.Now.ToString("HH:mm"));
            }
            if (end)
            {
                entity.EndTime = Utility.TimeStringToMinutes(DateTime.Now.ToString("HH:mm"));
            }
            if (entity.RestTime == null)
            {
                entity.RestTime = 60;
            }
            if (entity.BeginTime != null && entity.EndTime != null)
            {
                entity.OfficeTime = (int)entity.EndTime - (int)entity.BeginTime;
            }

            if (oldentity == null)
            {
                dao.Insert(entity);
            }
            else
            {
                dao.UpdatePrimaryKey(entity);
            }

            BeginTime.Text = Utility.MinutesToTimeString(entity.BeginTime);
            EndTime.Text = Utility.MinutesToTimeString(entity.EndTime);
        }

        protected void BeginButton_Click(object sender, EventArgs e)
        {
            UpdateWorkData(true, false);
        }

        protected void EndButton_Click(object sender, EventArgs e)
        {
            UpdateWorkData(false, true);
        }

        protected void EditButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Work/Edit.aspx?workDate=" + DateTime.Today.ToString(Utility.DATE_FORMAT_YYYYMMDD), false);
        }
    }
}
