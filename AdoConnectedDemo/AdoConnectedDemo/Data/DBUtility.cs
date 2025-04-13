using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using System.Data.SqlClient;

namespace AdoConnectedDemo.Data
{
    internal class DBUtility
    {
        static readonly string connectionString = @"Server = DESKTOP-EHHLEIA\SQLEXPRESS ; Database = hexaware ; Integrated Security=True ; MultipleActiveResultSets=true;";

        //Method opens connection
        public static SqlConnection GetConnection()
        {

            SqlConnection connectionObject = new SqlConnection(connectionString);
        
            try
            {
                connectionObject.Open();
                return connectionObject;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error Opening the Connection : {e.Message}");
                return null;
            }

        }



        //********

        //close
        public static void CloseDbConnection(SqlConnection connectionObject)
        {
            if (connectionObject != null)
            {
                try
                {
                    if (connectionObject.State != ConnectionState.Open)
                    {
                        connectionObject.Close();
                        connectionObject.Dispose();
                        Console.WriteLine("Comnection closed");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error Closing the Connection : {e.Message}");

                }

            }
            else
            {
                Console.WriteLine("Connection is already null.");

            }
        }


        //*********


    }
}
