using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace Kintai
{
    public partial class WorkTimeDao
    {
        public const string TABLE_NAME = "WorkTime";

        public static class COLUMN_LIST
        {
            #region Columns
            public const string UserId = "UserId";
            public const string WorkDate = "WorkDate";
            public const string WorkType = "WorkType";
            public const string BeginTime = "BeginTime";
            public const string EndTime = "EndTime";
            public const string RestTime = "RestTime";
            public const string OfficeTime = "OfficeTime";
            public const string WorkTime = "WorkTime";
            public const string WorkDetail = "WorkDetail";
            public const string CreateTimestamp = "CreateTimestamp";
            public const string CreateUserId = "CreateUserId";
            public const string UpdateTimestamp = "UpdateTimestamp";
            public const string UpdateUserId = "UpdateUserId";
            #endregion
        }

        public const string SQL_SELECT_PRIMARY_KEY = "select UserId,WorkDate,WorkType,BeginTime,EndTime,RestTime,OfficeTime,WorkTime,WorkDetail,CreateTimestamp,CreateUserId,UpdateTimestamp,UpdateUserId from WorkTime where UserId = @UserId AND WorkDate = @WorkDate";
        public const string SQL_INSERT = "insert into WorkTime(UserId,WorkDate,WorkType,BeginTime,EndTime,RestTime,OfficeTime,WorkTime,WorkDetail,CreateTimestamp,CreateUserId,UpdateTimestamp,UpdateUserId)values(@UserId,@WorkDate,@WorkType,@BeginTime,@EndTime,@RestTime,@OfficeTime,@WorkTime,@WorkDetail,@CreateTimestamp,@CreateUserId,@UpdateTimestamp,@UpdateUserId)";
        public const string SQL_UPDATE_PRIMARY_KEY = "update WorkTime set WorkType=@WorkType,BeginTime=@BeginTime,EndTime=@EndTime,RestTime=@RestTime,OfficeTime=@OfficeTime,WorkTime=@WorkTime,WorkDetail=@WorkDetail,CreateTimestamp=@CreateTimestamp,CreateUserId=@CreateUserId,UpdateTimestamp=@UpdateTimestamp,UpdateUserId=@UpdateUserId where UserId = @UserId AND WorkDate = @WorkDate";
        public const string SQL_DELETE_PRIMARY_KEY = "delete from WorkTime where UserId = @UserId AND WorkDate = @WorkDate";

        public virtual DatabaseAccess Access { get; set; }
        public virtual SqlConnection Connection { get; set; }

        public WorkTimeDao()
        {
            Access = new DatabaseAccess();
        }

        public WorkTimeDao(SqlConnection conn)
            : this()
        {
            Connection = conn;
        }

        protected virtual SqlConnection GetConnection()
        {
            return Connection;
        }

        public WorkTimeEntity SelectPrimaryKey(WorkTimeEntity filter)
        {
            Dictionary<string, object> param = FillPrimaryKey(filter);

            SqlConnection conn = GetConnection();

            SqlCommand command = conn.CreateCommand();
            command.CommandText = SQL_SELECT_PRIMARY_KEY;
            if (param != null)
            {
                foreach (var key in param.Keys)
                {
                    command.Parameters.AddWithValue("@" + key, param[key]);
                }
            }

            List<Dictionary<string, object>> list = Access.Select(conn, SQL_SELECT_PRIMARY_KEY, param);
            if (list.Count == 0)
            {
                return null;
            }

            WorkTimeEntity record2 = FillEntyty(list[0]);

            return record2;
        }

        public List<WorkTimeEntity> SelectWhere(string sql, Dictionary<string, object> param)
        {
            SqlConnection conn = GetConnection();

            List<Dictionary<string, object>> list = Access.Select(conn, sql, param);
            List<WorkTimeEntity> list2 = new List<WorkTimeEntity>();
            foreach (var item in list)
            {
                WorkTimeEntity record2 = FillEntyty(item);
                list2.Add(record2);
            }

            return list2;
        }

        public void Insert(WorkTimeEntity filter)
        {
            Dictionary<string, object> param = FillParam(filter);

            SqlConnection conn = GetConnection();
            Access.Update(conn, SQL_INSERT, param);
        }

        public int UpdatePrimaryKey(WorkTimeEntity filter)
        {
            Dictionary<string, object> param = FillParam(filter);

            SqlConnection conn = GetConnection();
            return Access.Update(conn, SQL_UPDATE_PRIMARY_KEY, param);
        }

        public int DeletePrimaryKey(WorkTimeEntity filter)
        {
            Dictionary<string, object> param = FillPrimaryKey(filter);

            SqlConnection conn = GetConnection();
            return Access.Update(conn, SQL_DELETE_PRIMARY_KEY, param);
        }

        protected virtual Dictionary<string, object> FillPrimaryKey(WorkTimeEntity filter)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            #region PrimaryKeySet
            param[COLUMN_LIST.UserId] = filter.UserId;
            param[COLUMN_LIST.WorkDate] = filter.WorkDate;
            #endregion

            return param;
        }

        protected virtual Dictionary<string, object> FillParam(WorkTimeEntity filter)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            #region ParamSet
            param[COLUMN_LIST.UserId] = filter.UserId;
            param[COLUMN_LIST.WorkDate] = filter.WorkDate;
            param[COLUMN_LIST.WorkType] = filter.WorkType;
            param[COLUMN_LIST.BeginTime] = filter.BeginTime;
            param[COLUMN_LIST.EndTime] = filter.EndTime;
            param[COLUMN_LIST.RestTime] = filter.RestTime;
            param[COLUMN_LIST.OfficeTime] = filter.OfficeTime;
            param[COLUMN_LIST.WorkTime] = filter.WorkTime;
            param[COLUMN_LIST.WorkDetail] = filter.WorkDetail;
            param[COLUMN_LIST.CreateTimestamp] = filter.CreateTimestamp;
            param[COLUMN_LIST.CreateUserId] = filter.CreateUserId;
            param[COLUMN_LIST.UpdateTimestamp] = filter.UpdateTimestamp;
            param[COLUMN_LIST.UpdateUserId] = filter.UpdateUserId;
            #endregion

            return param;
        }

        protected virtual WorkTimeEntity FillEntyty(Dictionary<string, object> record)
        {
            WorkTimeEntity record2 = new WorkTimeEntity();

            #region EntytySet
            record2.UserId = (string)record[COLUMN_LIST.UserId];
            record2.WorkDate = (DateTime?)record[COLUMN_LIST.WorkDate];
            record2.WorkType = (int?)record[COLUMN_LIST.WorkType];
            record2.BeginTime = (int?)record[COLUMN_LIST.BeginTime];
            record2.EndTime = (int?)record[COLUMN_LIST.EndTime];
            record2.RestTime = (int?)record[COLUMN_LIST.RestTime];
            record2.OfficeTime = (int?)record[COLUMN_LIST.OfficeTime];
            record2.WorkTime = (int?)record[COLUMN_LIST.WorkTime];
            record2.WorkDetail = (string)record[COLUMN_LIST.WorkDetail];
            record2.CreateTimestamp = (DateTime?)record[COLUMN_LIST.CreateTimestamp];
            record2.CreateUserId = (string)record[COLUMN_LIST.CreateUserId];
            record2.UpdateTimestamp = (DateTime?)record[COLUMN_LIST.UpdateTimestamp];
            record2.UpdateUserId = (string)record[COLUMN_LIST.UpdateUserId];
            #endregion

            return record2;
        }

        #region DatabaseAccess
        public class DatabaseAccess
        {
            public virtual List<Dictionary<string, object>> Select(SqlConnection conn, string sql, Dictionary<string, object> param)
            {
                if (sql == null)
                {
                    throw new ArgumentNullException("sql");
                }

                SqlCommand command = conn.CreateCommand();
                command.CommandText = sql;
                if (param != null)
                {
                    foreach (var key in param.Keys)
                    {
                        command.Parameters.AddWithValue("@" + key, param[key]);
                    }
                }
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    int fields = reader.FieldCount;
                    List<string> columnNames = new List<string>();
                    for (int i = 0; i < fields; i++)
                    {
                        columnNames.Add(reader.GetName(i));
                    }
                    List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
                    while (reader.Read())
                    {
                        Dictionary<string, object> record = new Dictionary<string, object>();
                        for (int i = 0; i < fields; i++)
                        {
                            if (reader.IsDBNull(i))
                            {
                                record.Add(columnNames[i], null);
                            }
                            else if (reader.GetFieldType(i) == typeof(String))
                            {
                                record.Add(columnNames[i], reader.GetString(i));
                            }
                            else if (reader.GetFieldType(i) == typeof(Int32))
                            {
                                record.Add(columnNames[i], reader.GetInt32(i));
                            }
                            else if (reader.GetFieldType(i) == typeof(DateTime))
                            {
                                record.Add(columnNames[i], reader.GetDateTime(i));
                            }
                            else
                            {
                                record.Add(columnNames[i], reader.GetValue(i));
                            }
                        }
                        list.Add(record);
                    }
                    return list;
                }
            }

            public virtual int Update(SqlConnection conn, string sql, Dictionary<string, object> param)
            {
                if (sql == null)
                {
                    throw new ArgumentNullException("sql");
                }

                SqlCommand command = conn.CreateCommand();
                command.CommandText = sql;
                if (param != null)
                {
                    foreach (var key in param.Keys)
                    {
                        object val = param[key];
                        if (val == null)
                        {
                            val = DBNull.Value;
                        }
                        command.Parameters.AddWithValue("@" + key, val);
                    }
                }
                int cnt = command.ExecuteNonQuery();
                return cnt;
            }
        }
        #endregion

    }
}
