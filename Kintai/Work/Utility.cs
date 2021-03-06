﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Net.Mail;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Data.SqlClient;

namespace Kintai.Work
{
    public static class Utility
    {
        public const string DATE_FORMAT_YYYYMM = "yyyy-MM";
        public const string DATE_FORMAT_YYYYMMDD = "yyyy-MM-dd";

        public enum WorkType
        {
            Empty,
            Normal,
            Holiday,
            PaidVacation,
            HalfVacation,
            WorkingHoliday,
            PaidLeave,
            NoDay
        }

        public static void SendMail(string from, string to, string subject, string content)
        {
            string host = WebConfigurationManager.AppSettings["MailSmtpHost"];
            int port = int.Parse(WebConfigurationManager.AppSettings["MailSmtpPort"]);
            bool ssl = int.Parse(WebConfigurationManager.AppSettings["MailSmtpSsl"]) != 0;
            string user = WebConfigurationManager.AppSettings["MailSmtpUser"];
            string pass = WebConfigurationManager.AppSettings["MailSmtpPassword"];

            if (from == null)
            {
                from = WebConfigurationManager.AppSettings["MailDefaultFromAddress"];
            }
            using (MailMessage msg = new MailMessage(from, to, subject, content))
            {
                using (SmtpClient smtp = new SmtpClient())
                {
                    smtp.EnableSsl = ssl;
                    smtp.Host = host;
                    smtp.Port = port;
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.Credentials = new System.Net.NetworkCredential(user, pass);
                    smtp.Send(msg);
                }
            }
        }

        public static bool LoadMailTemplate(string path, out string subject, out string message)
        {
            string[] mailLine = File.ReadAllLines(path, Encoding.Default);
            subject = null;
            message = null;
            int mode = 0;
            foreach (var line in mailLine)
            {
                if (line.StartsWith(";"))
                {
                    continue;
                }
                if (line == "[subject]")
                {
                    mode = 1;
                    continue;
                }
                else if (line == "[message]")
                {
                    mode = 2;
                    continue;
                }
                if (mode == 1 && line.Length > 0)
                {
                    subject = line;
                }
                else if (mode == 2)
                {
                    message += line + "\r\n";
                }
            }
            if (string.IsNullOrEmpty(subject) || string.IsNullOrEmpty(message))
            {
                return false;
            }
            return true;
        }

        public static string MinutesToTimeString(int? minutes)
        {
            if (minutes == null)
            {
                return null;
            }
            return string.Format("{0:00}:{1:00}", ((int)minutes / 60), ((int)minutes % 60));
        }

        public static int? TimeStringToMinutes(string time)
        {
            if (time == null)
            {
                return null;
            }
            if (!Regex.IsMatch(time, "[\\d]+:[\\d]+"))
            {
                return null;
            }
            var parts = time.Split(':');
            int minutes = int.Parse(parts[0]) * 60 + int.Parse(parts[1]);
            return minutes;
        }

        public static string WorkTypeToString(int? type, bool print = false)
        {
            if (type == null)
            {
                return null;
            }
            if (type == 1)
            {
                if (print)
                {
                    return "";
                }
                return "平日";
            }
            else if (type == 2)
            {
                return "休日";
            }
            else if (type == 3)
            {
                return "有給";
            }
            else if (type == 4)
            {
                return "半休";
            }
            else if (type == 5)
            {
                return "休出";
            }
            else if (type == 6)
            {
                return "休暇";
            }
            else if (type == 7)
            {
                return "なし";
            }
            return null;
        }

        public static Dictionary<DateTime, HolidayInfoEntity> GetHoliday(DateTime targetMonth)
        {
            SqlConnection conn = ThreadConnectionHolder.GetConnection();
            HolidayInfoDao dao2 = new HolidayInfoDao(conn);
            HolidayInfoEntity holidayEntity = new HolidayInfoEntity();
            holidayEntity.Holiday1 = targetMonth.Date;
            holidayEntity.Holiday2 = targetMonth.AddMonths(1).AddDays(-1);
            List<HolidayInfoEntity> HolidayInRange = dao2.SelectRange(holidayEntity);
            Dictionary<DateTime, HolidayInfoEntity> holidayMap = new Dictionary<DateTime, HolidayInfoEntity>();
            foreach (var item in HolidayInRange)
            {
                holidayMap.Add(item.Holiday.Value, item);
            }
            return holidayMap;
        }

        public static void FixedMonthlyList(List<WorkTimeEntity> list, DateTime startDate)
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

    }
}