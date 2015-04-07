using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Threading;
using System.Web.Configuration;

namespace Kintai
{
    public class ThreadConnectionHolder
    {
        private ThreadConnectionHolder() { }

        protected static ThreadLocal<bool> Connected = new ThreadLocal<bool>();
        protected static ThreadLocal<SqlConnection> ConnectionObj = new ThreadLocal<SqlConnection>();

        public static void Init()
        {
            if (ConnectionObj.Value == null)
            {
                string connstr = WebConfigurationManager.ConnectionStrings["ProjectServices"].ConnectionString;

                SqlConnection conn = new SqlConnection();
                conn.ConnectionString = connstr;
                ConnectionObj.Value = conn;
            }
        }

        public static SqlConnection GetConnection()
        {
            if (!Connected.Value)
            {
                Init();
                ConnectionObj.Value.Open();
                Connected.Value = true;
            }

            return ConnectionObj.Value;
        }

        public static void Close()
        {
            if(ConnectionObj.Value != null)
            {
                ConnectionObj.Value.Dispose();
            }
            ConnectionObj.Value = null;
            Connected.Value = false;
        }

    }
}