using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualArtGalleryNew.Util
{
    public class DBConnUtil
    {
        static readonly string connectionString = @"Server =DESKTOP-EHHLEIA\SQLEXPRESS ; Database = VirtualAGNew ; Integrated Security=True ; MultipleActiveResultSets=true;";
        public static SqlConnection GetConnection()
        {
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
