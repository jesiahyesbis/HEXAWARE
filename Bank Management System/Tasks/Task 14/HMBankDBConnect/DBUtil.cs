using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMBankDBConnect
{
    public class DBUtil
    {
        static readonly string connectionString = @"Server =DESKTOP-EHHLEIA\SQLEXPRESS; Database = Dbconnection ; Integrated Security =True ; MultipleActiveResultSets=true;";



        //Method to open Connection

        public static SqlConnection GetConnection()
        {

            SqlConnection ConnectionObject = new SqlConnection(connectionString);
            try
            {
                ConnectionObject.Open();
                return ConnectionObject;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error Opening the connection:{ex.Message}");
                return null;
            }
        }

        //Method to close connction
        public static void CloseDBConnection(SqlConnection ConnectionObject)
        {

            if (ConnectionObject.State != null)
            {
                try
                {
                    if (ConnectionObject.State != ConnectionState.Open)
                    {
                        ConnectionObject.Close();
                        ConnectionObject.Dispose();
                        Console.WriteLine("Connection closed");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error closing connection {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("connection is already null");
            }

        }
    }
}
    /*public class DBUtility
        {
            static readonly string connectionString = @"Server =DESKTOP-EDS7OFD\SQLEXPRESS; Database = hex ; Integrated Security =True ; MultipleActiveResultSets=true;";



            //Method to open Connection

            public static SqlConnection GetConnection()
            {

                SqlConnection ConnectionObject = new SqlConnection(connectionString);
                try
                {
                    ConnectionObject.Open();
                    return ConnectionObject;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error Opening the connection:{ex.Message}");
                    return null;
                }
            }

            //Method to close connction
            public static void CloseDBConnection(SqlConnection ConnectionObject)
            {

                if (ConnectionObject.State != null)
                {
                    try
                    {
                        if (ConnectionObject.State != ConnectionState.Open)
                        {
                            ConnectionObject.Close();
                            ConnectionObject.Dispose();
                            Console.WriteLine("Connection closed");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error closing connection {ex.Message}");
                    }
                }
                else
                {
                    Console.WriteLine("connection is already null");
                }

            }
        }
    }*/


