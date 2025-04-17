using System;
using System.Data;
using System.Data.SqlClient;
namespace HMBankDBConnect
{
    public static class DBUtil
    {
        private static string connectionString = @"Server=DESKTOP-EHHLEIA\SQLEXPRESS;Database=BankingSystem;Integrated Security=True;";

        public static SqlConnection GetDBConn()
        {
            SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();
                return connection;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error opening database connection: {ex.Message}");
                throw;
            }
        }

        public static void CloseDBConn(SqlConnection connection)
        {
            try
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error closing database connection: {ex.Message}");
            }
        }

        public static long GetLastInsertedAccountNumber()
        {
            using (SqlConnection connection = GetDBConn())
            {
                string query = "SELECT ISNULL(MAX(AccountNumber), 1000) FROM Accounts";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    return Convert.ToInt64(command.ExecuteScalar());
                }
            }
        }

        public static int GetLastInsertedCustomerId()
        {
            using (SqlConnection connection = GetDBConn())
            {
                string query = "SELECT ISNULL(MAX(CustomerID), 0) FROM Customers";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    return Convert.ToInt32(command.ExecuteScalar());
                }
            }
        }
    }

}


