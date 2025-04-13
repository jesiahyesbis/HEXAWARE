using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace VirtualArtGallery.util
{
    public static class DBConnUtil
    {
        public static SqlConnection GetConnection()
        {
            // Gets the pre-built connection string
            return new SqlConnection(DBPropertyUtil.GetPropertyString());
        }
    }
}
