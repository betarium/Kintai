using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Text.RegularExpressions;
using OfficeOpenXml;
using System.IO;

namespace Kintai.Work
{
    public partial class Month : System.Web.UI.Page
    {
        protected DateTime TargetMonthDate { get; set; }

        protected List<WorkTimeEntity> WorkTimeList { get; set; }

        protected static void CalcTotal(List<WorkTimeEntity> dayList, out int totalOfficeTime, out int totalWorkTime, out int totalRestTime)
        {
            totalOfficeTime = 0;
            totalWorkTime = 0;
            totalRestTime = 0;

            foreach (var dayItem in dayList)
            {
                if (dayItem.BeginTime == null || dayItem.EndTime == null)
                {
                    continue;
                }
                int officeTime = (dayItem.EndTime ?? 0) - (dayItem.BeginTime ?? 0);
                int restTime = (dayItem.RestTime ?? 0);
                totalOfficeTime += officeTime;
                totalWorkTime += officeTime - restTime;
                totalRestTime += restTime;
            }
        }

        protected static void FixedMonthlyList(List<WorkTimeEntity> list, DateTime startDate)
        {
            Dictionary<string, WorkTimeEntity> dataMap = new Dictionary<string, WorkTimeEntity>();
            foreach (var item in list)
            {
                string key = item.WorkDate.Value.ToString(Utility.DATE_FORMAT_YYYYMMDD);
                if (dataMap.ContainsKey(key))
                {
                    continue;
                }
                dataMap.Add(key, item);
            }

            List<WorkTimeEntity> dayList = new List<WorkTimeEntity>();
            for (int day = 1; day <= 31; day++)
            {
                DateTime newday = startDate.AddDays(day - 1);
                if (newday.Month != startDate.Month)
                {
                    break;
                }
                WorkTimeEntity entity = new WorkTimeEntity();
                var newymd = newday.ToString(Utility.DATE_FORMAT_YYYYMMDD);
                if (dataMap.ContainsKey(newymd))
                {
                    entity = dataMap[newymd];
                }
                entity.WorkDate = newday;
                dayList.Add(entity);
            }
            list.Clear();
            list.AddRange(dayList);
        }

        protected void LoadData()
        {
            var user = Membership.GetUser();

            Dictionary<string, object> sqlparam = new Dictionary<string, object>();
            sqlparam.Add("UserId", user.ProviderUserKey.ToString());
            sqlparam.Add("WorkDateBegin", TargetMonthDate);
            sqlparam.Add("WorkDateEnd", TargetMonthDate.AddMonths(1).AddDays(-1));

            WorkTimeDao dao = new WorkTimeDao();
            dao.Connection = ThreadConnectionHolder.GetConnection();
            List<WorkTimeEntity> dayList = dao.SelectWhere("select * from " + WorkTimeDao.TABLE_NAME + " where UserId = @UserId and WorkDate >= @WorkDateBegin and WorkDate <= @WorkDateEnd order by WorkDate ", sqlparam);
            //Dictionary<string, WorkTimeEntity> dataMap = new Dictionary<string, WorkTimeEntity>();
            //foreach (var item in list)
            //{
            //    string key = item.WorkDate.Value.ToString(Utility.DATE_FORMAT_YYYYMMDD);
            //    if (dataMap.ContainsKey(key))
            //    {
            //        continue;
            //    }
            //    dataMap.Add(key, item);
            //}

            //DateTime startDate = TargetMonthDate;
            //startDate = new DateTime(startDate.Year, startDate.Month, 1);
            //List<WorkTimeEntity> dayList = new List<WorkTimeEntity>();
            //for (int day = 1; day <= 31; day++)
            //{
            //    DateTime newday = startDate.AddDays(day - 1);
            //    if (newday.Month != startDate.Month)
            //    {
            //        break;
            //    }
            //    WorkTimeEntity entity = new WorkTimeEntity();
            //    var newymd = newday.ToString(Utility.DATE_FORMAT_YYYYMMDD);
            //    if (dataMap.ContainsKey(newymd))
            //    {
            //        entity = dataMap[newymd];
            //    }
            //    entity.WorkDate = newday;
            //    dayList.Add(entity);
            //}

            FixedMonthlyList(dayList, TargetMonthDate);

            int totalOfficeTime = 0;
            int totalWorkTime = 0;
            int totalRestTime = 0;
            CalcTotal(dayList, out totalOfficeTime, out totalWorkTime, out totalRestTime);

            /*
            foreach (var dayItem in dayList)
            {
                if (dayItem.BeginTime == null || dayItem.EndTime == null)
                {
                    continue;
                }
                int officeTime = (dayItem.EndTime ?? 0) - (dayItem.BeginTime ?? 0);
                int restTime = (dayItem.RestTime ?? 0);
                totalOfficeTime += officeTime;
                totalWorkTime += officeTime - restTime;
                totalRestTime += restTime;
            }
             * */

            int workDay = 0;
            foreach (var day in dayList)
            {
                if (day.WorkType == 1 && day.WorkTime != null && day.WorkTime > 0)
                {
                    workDay++;
                }
            }

            TotalOfficeTime.Text = Utility.MinutesToTimeString(totalOfficeTime);
            TotalWorkTime.Text = Utility.MinutesToTimeString(totalWorkTime);
            TotalRestTime.Text = Utility.MinutesToTimeString(totalRestTime);

            TotalWorkTime2.Text = Utility.MinutesToTimeString(totalWorkTime);
            TotalRestTime2.Text = Utility.MinutesToTimeString(totalRestTime);
            TotalOverTime2.Text = Utility.MinutesToTimeString(Math.Max(totalWorkTime - (8 * 60 * workDay), 0));

            WorkTimeList = dayList;

            TargetMonthHead.Text = TargetMonthDate.ToString("yyyy年MM月");
            DateList.DataSource = WorkTimeList;
            DateList.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            WorkTimeList = new List<WorkTimeEntity>();

            DateTime targetMonthTemp = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);

            if (Regex.IsMatch(TargetMonth.Value, "[\\d]+-[\\d]+"))
            {
                DateTime.TryParseExact(TargetMonth.Value + "-01", Utility.DATE_FORMAT_YYYYMMDD, null, System.Globalization.DateTimeStyles.None, out targetMonthTemp);
            }
            TargetMonthDate = targetMonthTemp;

            string targetMonthStr = TargetMonthDate.ToString(Utility.DATE_FORMAT_YYYYMM);
            TargetMonth.Value = targetMonthStr;

            if (!IsPostBack)
            {
                LoadData();
            }
        }

        /*
        public bool ParseWorkTime(string workTime, out string workTimeNormal, out int hour, out int minute)
        {
            workTimeNormal = null;
            hour = 0;
            minute = 0;
            if (!Regex.IsMatch(workTime, "[\\d]\\d:[\\d]\\d"))
            {
                return false;
            }
            var items = workTime.Split(':');
            hour = int.Parse(items[0]);
            minute = int.Parse(items[1]);

            hour += minute / 60;
            minute %= 60;

            workTimeNormal = string.Format("{0:00}:{1:00}", hour, minute);
            return true;
        }
         * */

        protected void Page_PreLoad(object sender, EventArgs e)
        {
            if (!User.Identity.IsAuthenticated)
            {
                Response.Redirect("~/Account/Login.aspx", true);
                return;
            }
        }

        protected void EditButton_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            Response.Redirect("~/Work/Edit.aspx?WorkDate=" + button.CommandArgument, false);
        }

        protected void ReportButton_Click(object sender, EventArgs e)
        {
            LoadData();

            string fullName = (string)HttpContext.Current.Profile.GetPropertyValue("FullName");

            string path = Server.MapPath("/Config/WorkReport.xlsx");
            ExcelPackage excel = new ExcelPackage(new FileInfo(path));

            excel.Workbook.Worksheets[1].Cells[1, 5].Value = TargetMonthDate;
            excel.Workbook.Worksheets[1].Cells[2, 4].Value = fullName;

            int workDay = 0;
            for (int index = 0; index < WorkTimeList.Count; index++)
            {
                if (WorkTimeList[index].WorkDate.Value.DayOfWeek != DayOfWeek.Sunday && WorkTimeList[index].WorkDate.Value.DayOfWeek != DayOfWeek.Saturday)
                {
                    workDay++;
                }

                if (WorkTimeList[index].BeginTime == null || WorkTimeList[index].EndTime == null)
                {
                    excel.Workbook.Worksheets[1].Cells[9 + index, 5].Value = null;
                    excel.Workbook.Worksheets[1].Cells[9 + index, 6].Value = null;
                    excel.Workbook.Worksheets[1].Cells[9 + index, 8].Value = null;
                    if (WorkTimeList[index].WorkDate.Value.DayOfWeek == DayOfWeek.Sunday || WorkTimeList[index].WorkDate.Value.DayOfWeek == DayOfWeek.Saturday)
                    {
                        excel.Workbook.Worksheets[1].Cells[9 + index, 4].Value = "休日";
                    }
                    continue;
                }
                //excel.Workbook.Worksheets[1].Cells[9 + index, 4].Value = "";
                excel.Workbook.Worksheets[1].Cells[9 + index, 4].Value = Utility.WorkTypeToString(WorkTimeList[index].WorkType, true);
                excel.Workbook.Worksheets[1].Cells[9 + index, 5].Value = Utility.MinutesToTimeString(WorkTimeList[index].BeginTime);
                excel.Workbook.Worksheets[1].Cells[9 + index, 6].Value = Utility.MinutesToTimeString(WorkTimeList[index].EndTime);
                excel.Workbook.Worksheets[1].Cells[9 + index, 5].Value = new TimeSpan(0, (int)WorkTimeList[index].BeginTime, 0);
                excel.Workbook.Worksheets[1].Cells[9 + index, 6].Value = new TimeSpan(0, (int)WorkTimeList[index].EndTime, 0);
                //excel.Workbook.Worksheets[1].Cells[9 + index, 7].Value = Utility.MinutesToTimeString(WorkTimeList[index].EndTime - WorkTimeList[index].BeginTime);
                excel.Workbook.Worksheets[1].Cells[9 + index, 8].Value = new TimeSpan(0, (int)WorkTimeList[index].RestTime, 0);
            }

            excel.Workbook.Worksheets[1].Cells[42, 5].Value = workDay;

            string destDir = Server.MapPath("/Export/");
            destDir = Path.Combine(destDir, TargetMonthDate.ToString("yyyy年MM月"));
            if (!Directory.Exists(destDir))
            {
                Directory.CreateDirectory(destDir);
            }
            string destPath = Path.Combine(destDir, TargetMonthDate.ToString("M月") + "分_" + fullName + "_勤務表.xlsx");
            excel.SaveAs(new FileInfo(destPath));
        }

        protected void NextMonth_Click(object sender, EventArgs e)
        {
            TargetMonthDate = TargetMonthDate.AddMonths(1);
            TargetMonth.Value = TargetMonthDate.ToString(Utility.DATE_FORMAT_YYYYMM);

            LoadData();
        }

        protected void BeforeMonth_Click(object sender, EventArgs e)
        {
            TargetMonthDate = TargetMonthDate.AddMonths(-1);
            TargetMonth.Value = TargetMonthDate.ToString(Utility.DATE_FORMAT_YYYYMM);

            LoadData();
        }
    }
}