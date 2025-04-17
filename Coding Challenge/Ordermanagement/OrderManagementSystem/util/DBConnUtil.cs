using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace OrderManagementSystem.util
{



        public static class DBConnUtil
        {
            public static SqlConnection GetDBConn()
            {
                //string connectionString = DBPropertyUtil.GetConnectionString();
                string connectionString = @"Server =DESKTOP-EHHLEIA\SQLEXPRESS ; Database = OrderManagementDB; Integrated Security=True ; MultipleActiveResultSets=true;";
            
            SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                return connection;
            }

            public static void CloseConnection(SqlConnection connection)
            {
                if (connection != null && connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }
    
}
