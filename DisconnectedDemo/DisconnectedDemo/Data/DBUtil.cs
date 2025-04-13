using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace DisconnectedDemo.Data
{
    internal class DBUtil
    {
        static readonly string connectionString = @"Server =DESKTOP-EHHLEIA\SQLEXPRESS ; Database = hexaware ; Integrated Security =True ; MultipleActiveResultSets=true;";
        public static SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }

        public static SqlDataAdapter GetAdapter(string query)
        {
            SqlConnection conn = GetConnection();
            SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
            return adapter;
        }

    }
}
