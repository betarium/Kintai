using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Net.Mail;
using System.Web.Configuration;
using System.Web.Profile;
using System.Text.RegularExpressions;

namespace Kintai.Work
{
    public partial class Edit : System.Web.UI.Page
    {
        public DateTime TargetDate2 { get; set; }

        protected Dictionary<DateTime, HolidayInfoEntity> holidayMap = new Dictionary<DateTime, HolidayInfoEntity>();

        protected DateTime GetTargetWorkDate()
        {
            string workDateStr = WorkDate.Value;
            DateTime workDate = DateTime.Today;
            if (!string.IsNullOrEmpty(workDateStr))
            {
                workDate = DateTime.ParseExact(workDateStr, Utility.DATE_FORMAT_YYYYMMDD, null);
            }
            else
            {
                string workDateParam = Request["workDate"];
                if (!string.IsNullOrEmpty(workDateParam))
                {
                    workDate = DateTime.ParseExact(workDateParam, Utility.DATE_FORMAT_YYYYMMDD, null);
                }
            }
            WorkDate.Value = workDate.ToString(Utility.DATE_FORMAT_YYYYMMDD);
            TargetDate.Text = workDate.ToString("yyyy年MM月dd日(ddd)");
            TargetDate2 = workDate;
            return workDate;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Membership.GetUser() == null || !User.Identity.IsAuthenticated)
            {
                Response.Redirect("~/Account/Login.aspx", true);
                return;
            }

            DateTime workDate = GetTargetWorkDate();

            WorkTimeEntity entity = new WorkTimeEntity();

            var user = Membership.GetUser();
            entity.UserId = user.ProviderUserKey.ToString();
            entity.WorkDate = workDate;

            holidayMap = Utility.GetHoliday(workDate);

            if (!IsPostBack)
            {
                WorkTimeDao dao = new WorkTimeDao();
                dao.Connection = ThreadConnectionHolder.GetConnection();
                WorkTimeEntity oldentity = dao.SelectPrimaryKey(entity);

                if (string.IsNullOrEmpty(EditFlag.Value) && oldentity != null)
                {
                    WorkTypeDropDownList.SelectedValue = (oldentity.WorkType == null ? "" : oldentity.WorkType.ToString());
                    BeginTime.Text = Utility.MinutesToTimeString(oldentity.BeginTime);
                    EndTime.Text = Utility.MinutesToTimeString(oldentity.EndTime);
                    RestTime.Text = Utility.MinutesToTimeString(oldentity.RestTime);
                    WorkDetail.Text = oldentity.WorkDetail;
                    WorkTime.Text = Utility.MinutesToTimeString(oldentity.WorkTime);

                    EditFlag.Value = "1";
                }

                if (string.IsNullOrEmpty(EditFlag.Value) && oldentity == null)
                {
                    if (workDate.DayOfWeek == DayOfWeek.Saturday || workDate.DayOfWeek == DayOfWeek.Sunday)
                    {
                        WorkTypeDropDownList.SelectedIndex = 1;
                    }
                    else if (isHoliday(workDate) == 1)
                    {
                        WorkTypeDropDownList.SelectedIndex = 1;
                    }
                }
            }
        }

        protected void Page_PreLoad(object sender, EventArgs e)
        {
            if (!User.Identity.IsAuthenticated)
            {
                Response.Redirect("~/Account/Login.aspx", true);
                return;
            }
        }

        protected void SaveData()
        {
            DateTime workDate = GetTargetWorkDate();

            WorkTimeEntity entity = new WorkTimeEntity();

            var user = Membership.GetUser();
            entity.UserId = user.ProviderUserKey.ToString();
            entity.WorkDate = workDate;

            WorkTimeDao dao = new WorkTimeDao();
            dao.Connection = ThreadConnectionHolder.GetConnection();
            WorkTimeEntity oldentity = dao.SelectPrimaryKey(entity);

            if (oldentity != null)
            {
                entity = oldentity;
            }

            if (string.IsNullOrEmpty(WorkTypeDropDownList.SelectedValue))
            {
                entity.WorkType = null;
            }
            else
            {
                entity.WorkType = int.Parse(WorkTypeDropDownList.SelectedValue);
            }
            entity.BeginTime = Utility.TimeStringToMinutes(BeginTime.Text);
            entity.EndTime = Utility.TimeStringToMinutes(EndTime.Text);
            entity.RestTime = Utility.TimeStringToMinutes(RestTime.Text);
            if (entity.BeginTime == null || entity.EndTime == null)
            {
                entity.OfficeTime = null;
                entity.WorkTime = null;
            }
            else
            {
                entity.OfficeTime = entity.EndTime - entity.BeginTime;
                entity.WorkTime = entity.OfficeTime - (entity.RestTime ?? 0);
            }
            entity.WorkDetail = WorkDetail.Text;

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
            RestTime.Text = Utility.MinutesToTimeString(entity.RestTime);
            WorkTime.Text = Utility.MinutesToTimeString(entity.WorkTime);
        }

        protected void SaveButton_Click(object sender, EventArgs e)
        {
            SaveData();
            InfoMessage.Text = "保存しました。";
        }

        protected void MailButton_Click(object sender, EventArgs e)
        {
            SaveData();

            DateTime workDate = GetTargetWorkDate();

            var user = Membership.GetUser();

            //string mailDefaultFromAddress = WebConfigurationManager.AppSettings["MailDefaultFromAddress"];
            string mailFrom = user.Email;
            string mailForceFromAddress = WebConfigurationManager.AppSettings["MailForceFromAddress"];
            if (!string.IsNullOrEmpty(mailForceFromAddress))
            {
                mailFrom = mailForceFromAddress;
            }
            string mailReportAddress = WebConfigurationManager.AppSettings["MailReportAddress"];

            string subject = null;
            string message = null;
            string path = Server.MapPath("/Config/MailDaily.txt");

            Utility.LoadMailTemplate(path, out subject, out message);

            string fullName = (string)HttpContext.Current.Profile.GetPropertyValue("FullName");

            subject = subject.Replace("%WorkDate%", workDate.ToString("yyyy/MM/dd"));
            subject = subject.Replace("%UserName%", fullName);

            message = message.Replace("%BeginTime%", BeginTime.Text);
            message = message.Replace("%EndTime%", EndTime.Text);
            message = message.Replace("%RestTime%", RestTime.Text);
            //int? workTime = null;
            //if (Utility.TimeStringToMinutes(BeginTime.Text) != null && Utility.TimeStringToMinutes(EndTime.Text) != null)
            //{
            //    workTime = Utility.TimeStringToMinutes(BeginTime.Text) - Utility.TimeStringToMinutes(EndTime.Text);
            //}
            //message = message.Replace("%WorkTime%", (Utility.MinutesToTimeString(workTime) ?? ""));
            message = message.Replace("%WorkTime%", WorkTime.Text);
            message = message.Replace("%WorkDetail%", WorkDetail.Text);
            message = message.Replace("%WorkDate%", workDate.ToString("yyyy/MM/dd"));
            message = message.Replace("%UserName%", fullName);

            Utility.SendMail(mailFrom, mailReportAddress, subject, message);
            //Response.Redirect("/Work/Month.aspx");

            InfoMessage.Text = "メールを送信しました。";

        }

        protected int isHoliday(DateTime date)
        {
            return holidayMap.ContainsKey(date) ? 1 : 0;
        }

        protected void BeforeDay_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Work/Edit.aspx?workDate=" + TargetDate2.AddDays(-1).ToString(Utility.DATE_FORMAT_YYYYMMDD), false);
        }

        protected void NextDay_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Work/Edit.aspx?workDate=" + TargetDate2.AddDays(1).ToString(Utility.DATE_FORMAT_YYYYMMDD), false);
        }

        protected void EndTimeInput_Click(object sender, EventArgs e)
        {
            EndTime.Text = DateTime.Now.ToString("HH:mm");
        }

        protected void RestTime1Link_Click(object sender, EventArgs e)
        {
            RestTime.Text = "01:00";
        }

        protected void BeginTimeInput_Click(object sender, EventArgs e)
        {
            BeginTime.Text = DateTime.Now.ToString("HH:mm");
        }

    }
}