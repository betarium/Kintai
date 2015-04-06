using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace Kintai
{
    public partial class HolidayInfoDao
    {
        public const string TABLE_NAME = "HolidayInfo";

        public static class COLUMN_LIST
        {
            #region Columns
            public const string HolidayDate = "HolidayDate";
            public const string HolidayType = "HolidayType";
            public const string HolidayName = "HolidayName";
            public const string CreateTimestamp = "CreateTimestamp";
            public const string CreateUserId = "CreateUserId";
            public const string UpdateTimestamp = "UpdateTimestamp";
            public const string UpdateUserId = "UpdateUserId";
            #endregion
        }

        public const string SQL_SELECT_PRIMARY_KEY = "select HolidayDate,HolidayType,HolidayName,CreateTimestamp,CreateUserId,UpdateTimestamp,UpdateUserId from HolidayInfo where HolidayDate = @HolidayDate";
        public const string SQL_INSERT = "insert into HolidayInfo(HolidayDate,HolidayType,HolidayName,CreateTimestamp,CreateUserId,UpdateTimestamp,UpdateUserId)values(@HolidayDate,@HolidayType,@HolidayName,@CreateTimestamp,@CreateUserId,@UpdateTimestamp,@UpdateUserId)";
        public const string SQL_UPDATE_PRIMARY_KEY = "update HolidayInfo set HolidayType=@HolidayType,HolidayName=@HolidayName,CreateTimestamp=@CreateTimestamp,CreateUserId=@CreateUserId,UpdateTimestamp=@UpdateTimestamp,UpdateUserId=@UpdateUserId where HolidayDate = @HolidayDate";
        public const string SQL_DELETE_PRIMARY_KEY = "delete from HolidayInfo where HolidayDate = @HolidayDate";

        public virtual DatabaseAccess Access { get; set; }
        public virtual SqlConnection Connection { get; set; }

        public HolidayInfoDao()
        {
            Access = new DatabaseAccess();
        }

        public HolidayInfoDao(SqlConnection conn)
            : this()
        {
            Connection = conn;
        }

        protected virtual SqlConnection GetConnection()
        {
            return Connection;
        }

        public HolidayInfoEntity SelectPrimaryKey(HolidayInfoEntity filter)
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

            HolidayInfoEntity record2 = FillEntyty(list[0]);

            return record2;
        }

        public List<HolidayInfoEntity> SelectWhere(string sql, Dictionary<string, object> param)
        {
            SqlConnection conn = GetConnection();

            List<Dictionary<string, object>> list = Access.Select(conn, sql, param);
            List<HolidayInfoEntity> list2 = new List<HolidayInfoEntity>();
            foreach (var item in list)
            {
                HolidayInfoEntity record2 = FillEntyty(item);
                list2.Add(record2);
            }

            return list2;
        }

        public void Insert(HolidayInfoEntity filter)
        {
            Dictionary<string, object> param = FillParam(filter);

            SqlConnection conn = GetConnection();
            Access.Update(conn, SQL_INSERT, param);
        }

        public int UpdatePrimaryKey(HolidayInfoEntity filter)
        {
            Dictionary<string, object> param = FillParam(filter);

            SqlConnection conn = GetConnection();
            return Access.Update(conn, SQL_UPDATE_PRIMARY_KEY, param);
        }

        public int DeletePrimaryKey(HolidayInfoEntity filter)
        {
            Dictionary<string, object> param = FillPrimaryKey(filter);

            SqlConnection conn = GetConnection();
            return Access.Update(conn, SQL_DELETE_PRIMARY_KEY, param);
        }

        protected virtual Dictionary<string, object> FillPrimaryKey(HolidayInfoEntity filter)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            #region PrimaryKeySet
            param[COLUMN_LIST.HolidayDate] = filter.HolidayDate;
            #endregion

            return param;
        }

        protected virtual Dictionary<string, object> FillParam(HolidayInfoEntity filter)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            #region ParamSet
            param[COLUMN_LIST.HolidayDate] = filter.HolidayDate;
            param[COLUMN_LIST.HolidayType] = filter.HolidayType;
            param[COLUMN_LIST.HolidayName] = filter.HolidayName;
            param[COLUMN_LIST.CreateTimestamp] = filter.CreateTimestamp;
            param[COLUMN_LIST.CreateUserId] = filter.CreateUserId;
            param[COLUMN_LIST.UpdateTimestamp] = filter.UpdateTimestamp;
            param[COLUMN_LIST.UpdateUserId] = filter.UpdateUserId;
            #endregion

            return param;
        }

        protected virtual HolidayInfoEntity FillEntyty(Dictionary<string, object> record)
        {
            HolidayInfoEntity record2 = new HolidayInfoEntity();

            #region EntytySet
            record2.HolidayDate = (DateTime?)record[COLUMN_LIST.HolidayDate];
            record2.HolidayType = (int?)record[COLUMN_LIST.HolidayType];
            record2.HolidayName = (string)record[COLUMN_LIST.HolidayName];
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
