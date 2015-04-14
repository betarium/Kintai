using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using OfficeOpenXml;
using System.IO;
using System.Text.RegularExpressions;
using System.Data.SqlClient;

namespace Kintai.Work
{
    public partial class Export : System.Web.UI.Page
    {
        protected DateTime TargetStartDay;

        protected void Page_Load(object sender, EventArgs e)
        {
            DownloadLink.Visible = false;
            string month = TargetMonth.Text;
            if (month == "")
            {
                month = DateTime.Now.ToString(Utility.DATE_FORMAT_YYYYMM);
            }

            DateTime monthDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            if (Regex.IsMatch(month, "[\\d]+-[\\d]+"))
            {
                DateTime.TryParseExact(month + "-01", Utility.DATE_FORMAT_YYYYMMDD, null, System.Globalization.DateTimeStyles.None, out monthDate);
            }
            TargetMonth.Text = monthDate.ToString(Utility.DATE_FORMAT_YYYYMM);
            TargetStartDay = monthDate;
        }

        //protected static void FixedMonthlyList(List<WorkTimeEntity> list, DateTime startDate)
        //{
        //    Dictionary<string, WorkTimeEntity> dataMap = new Dictionary<string, WorkTimeEntity>();
        //    foreach (var item in list)
        //    {
        //        string key = item.WorkDate.Value.ToString(Utility.DATE_FORMAT_YYYYMMDD);
        //        if (dataMap.ContainsKey(key))
        //        {
        //            continue;
        //        }
        //        dataMap.Add(key, item);
        //    }

        //    List<WorkTimeEntity> dayList = new List<WorkTimeEntity>();
        //    for (int day = 1; day <= 31; day++)
        //    {
        //        DateTime newday = startDate.AddDays(day - 1);
        //        if (newday.Month != startDate.Month)
        //        {
        //            break;
        //        }
        //        WorkTimeEntity entity = new WorkTimeEntity();
        //        var newymd = newday.ToString(Utility.DATE_FORMAT_YYYYMMDD);
        //        if (dataMap.ContainsKey(newymd))
        //        {
        //            entity = dataMap[newymd];
        //        }
        //        entity.WorkDate = newday;
        //        dayList.Add(entity);
        //    }
        //    list.Clear();
        //    list.AddRange(dayList);
        //}

        protected void ReportButton_Click(object sender, EventArgs e)
        {
            var user = Membership.GetUser();

            Dictionary<string, object> sqlparam = new Dictionary<string, object>();
            sqlparam.Add("UserId", user.ProviderUserKey.ToString());
            sqlparam.Add("WorkDateBegin", TargetStartDay);
            sqlparam.Add("WorkDateEnd", TargetStartDay.AddMonths(1).AddDays(-1));

            WorkTimeDao dao = new WorkTimeDao();
            dao.Connection = ThreadConnectionHolder.GetConnection();
            List<WorkTimeEntity> dayList = dao.SelectWhere("select * from " + WorkTimeDao.TABLE_NAME + " where UserId = @UserId and WorkDate >= @WorkDateBegin and WorkDate <= @WorkDateEnd order by WorkDate ", sqlparam);

            Utility.FixedMonthlyList(dayList, TargetStartDay);

            Dictionary<DateTime, HolidayInfoEntity> holidayMap = Utility.GetHoliday(TargetStartDay);

            string fullName = (string)HttpContext.Current.Profile.GetPropertyValue("FullName");

            string path = Server.MapPath("/Config/WorkReport.xlsx");
            ExcelPackage excel = new ExcelPackage(new FileInfo(path));

            excel.Workbook.Worksheets[1].Cells[1, 5].Value = TargetStartDay;
            excel.Workbook.Worksheets[1].Cells[2, 4].Value = fullName;

            int workDay = 0;
            for (int index = 0; index < dayList.Count; index++)
            {
                DateTime workDate = dayList[index].WorkDate.Value;
                WorkTimeEntity entity = dayList[index];
                if (workDate.DayOfWeek != DayOfWeek.Sunday && workDate.DayOfWeek != DayOfWeek.Saturday)
                {
                    workDay++;
                }

                if (entity.BeginTime == null || entity.EndTime == null)
                {
                    excel.Workbook.Worksheets[1].Cells[9 + index, 5].Value = null;
                    excel.Workbook.Worksheets[1].Cells[9 + index, 6].Value = null;
                    excel.Workbook.Worksheets[1].Cells[9 + index, 8].Value = null;
                    if (workDate.DayOfWeek == DayOfWeek.Sunday || workDate.DayOfWeek == DayOfWeek.Saturday)
                    {
                        excel.Workbook.Worksheets[1].Cells[9 + index, 4].Value = "休日";
                    }
                    else if (holidayMap.ContainsKey(workDate))
                    {
                        excel.Workbook.Worksheets[1].Cells[9 + index, 4].Value = "休日";
                    }
                    continue;
                }

                excel.Workbook.Worksheets[1].Cells[9 + index, 4].Value = Utility.WorkTypeToString(entity.WorkType, true);
                excel.Workbook.Worksheets[1].Cells[9 + index, 5].Value = Utility.MinutesToTimeString(entity.BeginTime);
                excel.Workbook.Worksheets[1].Cells[9 + index, 6].Value = Utility.MinutesToTimeString(entity.EndTime);
                excel.Workbook.Worksheets[1].Cells[9 + index, 5].Value = new TimeSpan(0, (int)(entity.BeginTime ?? 0), 0);
                excel.Workbook.Worksheets[1].Cells[9 + index, 6].Value = new TimeSpan(0, (int)(entity.EndTime ?? 0), 0);
                excel.Workbook.Worksheets[1].Cells[9 + index, 8].Value = new TimeSpan(0, (int)(entity.RestTime ?? 0), 0);
            }

            excel.Workbook.Worksheets[1].Cells[42, 5].Value = workDay;

            string destDir = Server.MapPath("/Export/");
            destDir = Path.Combine(destDir, TargetStartDay.ToString("yyyy年MM月"));
            if (!Directory.Exists(destDir))
            {
                Directory.CreateDirectory(destDir);
            }
            string destPath = Path.Combine(destDir, TargetStartDay.ToString("M月") + "分_" + fullName + "_勤務表.xlsx");
            excel.SaveAs(new FileInfo(destPath));

            DownloadLink.Visible = true;
        }

        protected void DownloadLink_Click(object sender, EventArgs e)
        {
            string fullName = (string)HttpContext.Current.Profile.GetPropertyValue("FullName");

            string destDir = Server.MapPath("/Export/");
            destDir = Path.Combine(destDir, TargetStartDay.ToString("yyyy年MM月"));
            string destPath = Path.Combine(destDir, TargetStartDay.ToString("M月") + "分_" + fullName + "_勤務表.xlsx");

            DownloadLink.Visible = true;

            string fileNameEncode = HttpUtility.UrlEncode(Path.GetFileName(destPath));
            Response.ClearContent();

            Response.AddHeader("Content-Disposition", String.Format("attachment;filename={0}", fileNameEncode));
            Response.ContentType = "application/octet-stream";

            Response.WriteFile(destPath);
            Response.Flush();
            Response.End();
        }

    }
}