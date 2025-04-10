using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HMBankDBConnect
{
    internal class BankRepositoryImpl : IBankRepository
    {
        public void CreateAccount(Customer customer, string accType, decimal balance)
        {
            SqlConnection con = null;
            SqlTransaction txn = null;

            try
            {
                using (con = DBUtil.GetConnection())
                {
                    txn = con.BeginTransaction();

                    //string insertCustomer = @"INSERT INTO Customers (Name, Email, PhoneNumber, Address) 
                    //                          VALUES (@name, @email, @phone, @address);
                    //                          SELECT SCOPE_IDENTITY();";
                    string insertCustomer = @"INSERT INTO Customers (name, email, phone_number, address) 
                                              VALUES (@name, @email, @phone, @address);
                                              SELECT SCOPE_IDENTITY();";



                    SqlCommand cmd = new SqlCommand(insertCustomer, con, txn);
                    cmd.Parameters.AddWithValue("@name", customer.Name);
                    cmd.Parameters.AddWithValue("@email", customer.Email);
                    cmd.Parameters.AddWithValue("@phone", customer.PhoneNumber);
                    cmd.Parameters.AddWithValue("@address", customer.Address);

                    int customerId = Convert.ToInt32(cmd.ExecuteScalar());

                    string insertAccount = @"INSERT INTO Accounts (account_type, balance, customer_id)
                                             VALUES (@type, @balance, @custId)";
                    SqlCommand cmd2 = new SqlCommand(insertAccount, con, txn);
                    cmd2.Parameters.AddWithValue("@type", accType);
                    cmd2.Parameters.AddWithValue("@balance", balance);
                    cmd2.Parameters.AddWithValue("@custId", customerId);

                    cmd2.ExecuteNonQuery();

                    txn.Commit();
                }
            }
            catch (SqlException ex)
            {
                txn?.Rollback();
                throw ex;
            }
            catch (Exception ex)
            {
                txn?.Rollback();
                throw new Exception("Error creating account", ex);
            }
        }

        public List<Account> ListAccounts()
        {
            List<Account> accounts = new List<Account>();
            SqlConnection con = null;

            //string query = @"SELECT a.AccountNumber, a.AccountType, a.Balance, 
            //                        c.CustomerId, c.Name, c.Email, c.PhoneNumber, c.Address
            //                 FROM Accounts a
            //                 JOIN Customers c ON a.CustomerId = c.CustomerId";
            string query = @"SELECT a.account_number, a.account_type, a.balance, 
                                    c.customer_id, c.name, c.email, c.phone_number, c.address
                             FROM Accounts a
                             JOIN Customers c ON a.customer_id = c.customer_id";

            try
            {
                using (con = DBUtil.GetConnection())
                {
                    SqlCommand cmd = new SqlCommand(query, con);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Customer cust = new Customer(
                            (long)reader["CustomerId"],
                            reader["Name"].ToString(),
                            reader["Email"].ToString(),
                            reader["PhoneNumber"].ToString(),
                            reader["Address"].ToString()
                        );

                        string type = reader["AccountType"].ToString();
                        decimal balance = (decimal)reader["Balance"];
                        long accNum = (long)reader["AccountNumber"];

                        Account acc = type switch
                        {
                            "Savings" => new SavingsAccount(balance, 0.04m, cust),
                            "Current" => new CurrentAccount(balance, 1000m, cust),
                            _ => new ZeroBalanceAccount(cust)
                        };
                        acc.AccountNumber = accNum;
                        accounts.Add(acc);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }

            return accounts;
        }

        public decimal GetAccountBalance(long accountNumber)
        {
            SqlConnection con = null;
            decimal balance = 0;

            string query = "SELECT Balance FROM Accounts WHERE account_number = @accNum";

            try
            {
                using (con = DBUtil.GetConnection())
                {
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@accNum", accountNumber);

                    object result = cmd.ExecuteScalar();
                    if (result == null || result == DBNull.Value)
                        throw new Exception("Account not found");

                    balance = Convert.ToDecimal(result);
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }

            return balance;
        }





        public void Deposit(long accountNumber, decimal amount)
        {
            SqlConnection con = null;
            string query = "UPDATE Accounts SET balance = balance + @amt WHERE account_number = @accNum";

            try
            {
                using (con = DBUtil.GetConnection())
                {
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@amt", amount);
                    cmd.Parameters.AddWithValue("@accNum", accountNumber);

                    int rows = cmd.ExecuteNonQuery();
                    if (rows == 0)
                        throw new Exception("Deposit failed. Account not found.");
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public void Withdraw(long accountNumber, decimal amount)
        {
            SqlConnection con = null;
            string query = "UPDATE Accounts SET balance = balance - @amt WHERE account_number = @accNum AND balance >= @amt";

            try
            {
                using (con = DBUtil.GetConnection())
                {
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@amt", amount);
                    cmd.Parameters.AddWithValue("@accNum", accountNumber);

                    int rows = cmd.ExecuteNonQuery();
                    if (rows == 0)
                        throw new Exception("Withdrawal failed. Either account not found or insufficient balance.");
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public void Transfer(long fromAccountNumber, long toAccountNumber, decimal amount)
        {
            SqlConnection con = null;
            SqlTransaction txn = null;

            try
            {
                using (con = DBUtil.GetConnection())
                {
                    txn = con.BeginTransaction();

                    SqlCommand withdrawCmd = new SqlCommand("UPDATE Accounts SET balance = balance - @amt WHERE account_number = @from AND balance >= @amt", con, txn);
                    withdrawCmd.Parameters.AddWithValue("@amt", amount);
                    withdrawCmd.Parameters.AddWithValue("@from", fromAccountNumber);
                    if (withdrawCmd.ExecuteNonQuery() == 0)
                        throw new Exception("Transfer failed during withdrawal. Check balance or account.");

                    SqlCommand depositCmd = new SqlCommand("UPDATE Accounts SET balance = balance + @amt WHERE account_number = @to", con, txn);
                    depositCmd.Parameters.AddWithValue("@amt", amount);
                    depositCmd.Parameters.AddWithValue("@to", toAccountNumber);
                    if (depositCmd.ExecuteNonQuery() == 0)
                        throw new Exception("Transfer failed during deposit. Target account not found.");

                    txn.Commit();
                }
            }
            catch (SqlException ex)
            {
                txn?.Rollback();
                throw ex;
            }
            catch (Exception ex)
            {
                txn?.Rollback();
                throw new Exception("Transfer failed", ex);
            }
        }

        public Account GetAccountDetails(long accountNumber)
        {
            SqlConnection con = null;
            Account acc = null;

            string query = @"SELECT a.account_number, a.account_type, a.balance,
                                    c.customer_id, c.name, c.email, c.phone_number, c.address
                             FROM Accounts a
                             JOIN Customers c ON a.customer_id = c.customer_id
                             WHERE a.account_number = @accNum";

            try
            {
                using (con = DBUtil.GetConnection())
                {
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@accNum", accountNumber);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        Customer cust = new Customer(
                            (long)reader["CustomerId"],
                            reader["Name"].ToString(),
                            reader["Email"].ToString(),
                            reader["PhoneNumber"].ToString(),
                            reader["Address"].ToString()
                        );

                        string type = reader["AccountType"].ToString();
                        decimal balance = (decimal)reader["Balance"];

                        acc = type switch
                        {
                            "Savings" => new SavingsAccount(balance, 0.04m, cust),
                            "Current" => new CurrentAccount(balance, 1000m, cust),
                            _ => new ZeroBalanceAccount(cust)
                        };
                        acc.AccountNumber = (long)reader["AccountNumber"];
                    }
                    else
                    {
                        throw new Exception("Account not found");
                    }
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }

            return acc;
        }

        public List<Transaction> GetTransactions(long accountNumber, DateTime fromDate, DateTime toDate)
        {
            SqlConnection con = null;
            List<Transaction> transactions = new List<Transaction>();

            string query = @"SELECT * FROM Transactions WHERE account_number = @accNum AND transaction_date BETWEEN @from AND @to";

            try
            {
                using (con = DBUtil.GetConnection())
                {
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@accNum", accountNumber);
                    cmd.Parameters.AddWithValue("@from", fromDate);
                    cmd.Parameters.AddWithValue("@to", toDate);

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Transaction txn = new Transaction(
                            (long)reader["AccountNumber"],
                            reader["Description"].ToString(),
                            (DateTime)reader["TransactionDate"],
                            reader["TransactionType"].ToString(),
                            (decimal)reader["Amount"]
                        );
                        transactions.Add(txn);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }

            return transactions;
        }



        public void CalculateInterest()
        {
            SqlConnection con = null;

            string query = @"UPDATE Accounts SET balance = balance + (balance * 0.04)
                             WHERE account_type = 'Savings'";

            try
            {
                using (con = DBUtil.GetConnection())
                {
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }




    }
}


    






























       /* public class BankRepositoryImpl : IBankRepository
        {
            private readonly string connectionString = DBUtil.GetConnection();
            public void CreateAccount(Customer customer, string accType, decimal balance)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlTransaction transaction = connection.BeginTransaction();

                    try
                    {
                        // Insert customer
                        string customerQuery = @"INSERT INTO Customers (Name, Email, PhoneNumber, Address) 
                                      VALUES (@Name, @Email, @Phone, @Address);
                                      SELECT SCOPE_IDENTITY();";

                        SqlCommand customerCmd = new SqlCommand(customerQuery, connection, transaction);
                        customerCmd.Parameters.AddWithValue("@Name", customer.Name);
                        customerCmd.Parameters.AddWithValue("@Email", customer.Email);
                        customerCmd.Parameters.AddWithValue("@Phone", customer.PhoneNumber);
                        customerCmd.Parameters.AddWithValue("@Address", customer.Address);

                        decimal customerId = Convert.ToDecimal(customerCmd.ExecuteScalar());

                        // Insert account
                        string accountQuery = @"INSERT INTO Accounts (AccountType, Balance, CustomerId) 
                                      VALUES (@AccType, @Balance, @CustomerId)";

                        SqlCommand accountCmd = new SqlCommand(accountQuery, connection, transaction);
                        accountCmd.Parameters.AddWithValue("@AccType", accType);
                        accountCmd.Parameters.AddWithValue("@Balance", balance);
                        accountCmd.Parameters.AddWithValue("@CustomerId", customerId);

                        accountCmd.ExecuteNonQuery();

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }

            public List<Account> ListAccounts()
            {
                List<Account> accounts = new List<Account>();

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = @"SELECT a.AccountNumber, a.AccountType, a.Balance, 
                               c.CustomerId, c.Name, c.Email, c.PhoneNumber, c.Address
                               FROM Accounts a
                               JOIN Customers c ON a.CustomerId = c.CustomerId";

                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Customer customer = new Customer(
                                Convert.ToInt64(reader["CustomerId"]),
                                reader["Name"].ToString(),
                                reader["Email"].ToString(),
                                reader["PhoneNumber"].ToString(),
                                reader["Address"].ToString()
                            );

                            Account account = reader["AccountType"].ToString() switch
                            {
                                "Savings" => new SavingsAccount(
                                    Convert.ToDecimal(reader["Balance"]),
                                    0.04m, // Default interest rate
                                    customer
                                ),
                                "Current" => new CurrentAccount(
                                                                Convert.ToDecimal(reader["Balance"]),
                                                                1000m, // Default overdraft limit
                                                                customer
                                                            ),
                                _ => new ZeroBalanceAccount(customer)
                            };

                            account.AccountNumber = Convert.ToInt64(reader["AccountNumber"]);
                            accounts.Add(account);
                        }
                    }
                }

                return accounts;
            }

            public decimal GetAccountBalance(long accountNumber)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "SELECT Balance FROM Accounts WHERE AccountNumber = @AccountNumber";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@AccountNumber", accountNumber);

                    connection.Open();
                    object result = command.ExecuteScalar();

                    if (result == null || result == DBNull.Value)
                    {
                        throw new ArgumentException("Account not found");
                    }

                    return Convert.ToDecimal(result);
                }
            }

            public void Deposit(long accountNumber, decimal amount)
            {
                if (amount <= 0)
                    throw new ArgumentException("Deposit amount must be positive");

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlTransaction transaction = connection.BeginTransaction();

                    try
                    {
                        // Update balance
                        string updateQuery = @"UPDATE Accounts SET Balance = Balance + @Amount 
                                      WHERE AccountNumber = @AccountNumber";
                        SqlCommand updateCmd = new SqlCommand(updateQuery, connection, transaction);
                        updateCmd.Parameters.AddWithValue("@Amount", amount);
                        updateCmd.Parameters.AddWithValue("@AccountNumber", accountNumber);

                        int rowsAffected = updateCmd.ExecuteNonQuery();

                        if (rowsAffected == 0)
                            throw new ArgumentException("Account not found");

                        // Record transaction
                        string txnQuery = @"INSERT INTO Transactions 
                                    (AccountNumber, TransactionDate, TransactionType, Amount, Description)
                                    VALUES (@AccountNumber, @TransactionDate, 'Deposit', @Amount, @Description)";

                        SqlCommand txnCmd = new SqlCommand(txnQuery, connection, transaction);
                        txnCmd.Parameters.AddWithValue("@AccountNumber", accountNumber);
                        txnCmd.Parameters.AddWithValue("@TransactionDate", DateTime.Now);
                        txnCmd.Parameters.AddWithValue("@Amount", amount);
                        txnCmd.Parameters.AddWithValue("@Description", $"Deposit of {amount:C}");

                        txnCmd.ExecuteNonQuery();

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }

            public void Withdraw(long accountNumber, decimal amount)
            {
                if (amount <= 0)
                    throw new ArgumentException("Withdrawal amount must be positive");

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlTransaction transaction = connection.BeginTransaction();
                    try
                    {
                        // First check balance and account type
                        string accountQuery = @"SELECT AccountType, Balance FROM Accounts 
                                       WHERE AccountNumber = @AccountNumber";
                        SqlCommand accountCmd = new SqlCommand(accountQuery, connection, transaction);
                        accountCmd.Parameters.AddWithValue("@AccountNumber", accountNumber);

                        using (SqlDataReader reader = accountCmd.ExecuteReader())
                        {
                            if (!reader.Read())
                                throw new ArgumentException("Account not found");

                            string accountType = reader["AccountType"].ToString();
                            decimal balance = Convert.ToDecimal(reader["Balance"]);
                            reader.Close();

                            // Validate withdrawal based on account type
                            switch (accountType)
                            {
                                case "Savings":
                                    if (balance - amount < 500)
                                        throw new InvalidOperationException("Minimum balance of 500 must be maintained");
                                    break;
                                case "Current":
                                    // Current accounts might have overdraft, but we'll assume no overdraft for this example
                                    if (balance < amount)
                                        throw new InvalidOperationException("Insufficient funds");
                                    break;
                                case "ZeroBalance":
                                    if (balance < amount)
                                        throw new InvalidOperationException("Insufficient funds");
                                    break;
                            }
                        }

                        // Update balance
                        string updateQuery = @"UPDATE Accounts SET Balance = Balance - @Amount 
                                      WHERE AccountNumber = @AccountNumber";
                        SqlCommand updateCmd = new SqlCommand(updateQuery, connection, transaction);
                        updateCmd.Parameters.AddWithValue("@Amount", amount);
                        updateCmd.Parameters.AddWithValue("@AccountNumber", accountNumber);
                        updateCmd.ExecuteNonQuery();

                        // Record transaction
                        string txnQuery = @"INSERT INTO Transactions 
                                    (AccountNumber, TransactionDate, TransactionType, Amount, Description)
                                    VALUES (@AccountNumber, @TransactionDate, 'Withdrawal', @Amount, @Description)";

                        SqlCommand txnCmd = new SqlCommand(txnQuery, connection, transaction);
                        txnCmd.Parameters.AddWithValue("@AccountNumber", accountNumber);
                        txnCmd.Parameters.AddWithValue("@TransactionDate", DateTime.Now);
                        txnCmd.Parameters.AddWithValue("@Amount", amount);
                        txnCmd.Parameters.AddWithValue("@Description", $"Withdrawal of {amount:C}");

                        txnCmd.ExecuteNonQuery();

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }

            public void Transfer(long fromAccountNumber, long toAccountNumber, decimal amount)
            {
                if (amount <= 0)
                    throw new ArgumentException("Transfer amount must be positive");

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlTransaction transaction = connection.BeginTransaction();

                    try
                    {
                        // First withdraw from source account
                        Withdraw(fromAccountNumber, amount);
                        // Then deposit to destination account
                        Deposit(toAccountNumber, amount);

                        // Record transfer transaction
                        string txnQuery = @"INSERT INTO Transactions 
                                    (AccountNumber, TransactionDate, TransactionType, Amount, Description)
                                    VALUES (@FromAccountNumber, @TransactionDate, 'Transfer', @Amount, 
                                    'Transfer to account ' + CAST(@ToAccountNumber AS VARCHAR(20)))";

                        SqlCommand txnCmd = new SqlCommand(txnQuery, connection, transaction);
                        txnCmd.Parameters.AddWithValue("@FromAccountNumber", fromAccountNumber);
                        txnCmd.Parameters.AddWithValue("@TransactionDate", DateTime.Now);
                        txnCmd.Parameters.AddWithValue("@Amount", amount);
                        txnCmd.Parameters.AddWithValue("@ToAccountNumber", toAccountNumber);
                        txnCmd.ExecuteNonQuery();

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }

            public Account GetAccountDetails(long accountNumber)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = @"SELECT a.AccountNumber, a.AccountType, a.Balance, 
                            c.CustomerId, c.Name, c.Email, c.PhoneNumber, c.Address
                            FROM Accounts a
                            JOIN Customers c ON a.CustomerId = c.CustomerId
                            WHERE a.AccountNumber = @AccountNumber";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@AccountNumber", accountNumber);
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Customer customer = new Customer(
                                Convert.ToInt64(reader["CustomerId"]),
                                reader["Name"].ToString(),
                                reader["Email"].ToString(),
                                reader["PhoneNumber"].ToString(),
                                reader["Address"].ToString()
                            );

                            Account account = reader["AccountType"].ToString() switch
                            {
                                "Savings" => new SavingsAccount(
                                    Convert.ToDecimal(reader["Balance"]),
                                    0.04m, // Default interest rate
                                    customer
                                ),
                                "Current" => new CurrentAccount(
                                    Convert.ToDecimal(reader["Balance"]),
                                    1000m, // Default overdraft limit
                                    customer
                                ),
                                _ => new ZeroBalanceAccount(customer)
                            };

                            account.AccountNumber = Convert.ToInt64(reader["AccountNumber"]);
                            return account;
                        }
                        else
                        {
                            throw new ArgumentException("Account not found");
                        }
                    }
                }
            }

            public List<Transaction> GetTransactions(long accountNumber, DateTime fromDate, DateTime toDate)
            {
                List<Transaction> transactions = new List<Transaction>();

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = @"SELECT TransactionDate, TransactionType, Amount, Description
                            FROM Transactions
                            WHERE AccountNumber = @AccountNumber
                            AND TransactionDate BETWEEN @FromDate AND @ToDate
                            ORDER BY TransactionDate";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@AccountNumber", accountNumber);
                    command.Parameters.AddWithValue("@FromDate", fromDate);
                    command.Parameters.AddWithValue("@ToDate", toDate);
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            transactions.Add(new Transaction(
                                accountNumber,
                                reader["Description"].ToString(),
                                Convert.ToDateTime(reader["TransactionDate"]),
                                reader["TransactionType"].ToString(),
                                Convert.ToDecimal(reader["Amount"])
                            ));
                        }
                    }
                }

                return transactions;
            }

            public void CalculateInterest()
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlTransaction transaction = connection.BeginTransaction();

                    try
                    {
                        // Get all savings accounts
                        string accountsQuery = @"SELECT AccountNumber, Balance FROM Accounts 
                                        WHERE AccountType = 'Savings'";
                        SqlCommand accountsCmd = new SqlCommand(accountsQuery, connection, transaction);

                        using (SqlDataReader reader = accountsCmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                long accountNumber = Convert.ToInt64(reader["AccountNumber"]);
                                decimal balance = Convert.ToDecimal(reader["Balance"]);
                                decimal interest = balance * 0.04m / 12; // 4% annual interest, monthly calculation

                                // Close the reader before executing updates
                                reader.Close();

                                // Update balance with interest
                                string updateQuery = @"UPDATE Accounts SET Balance = Balance + @Interest 
                                              WHERE AccountNumber = @AccountNumber";
                                SqlCommand updateCmd = new SqlCommand(updateQuery, connection, transaction);
                                updateCmd.Parameters.AddWithValue("@Interest", interest);
                                updateCmd.Parameters.AddWithValue("@AccountNumber", accountNumber);
                                updateCmd.ExecuteNonQuery();

                                // Record interest transaction
                                string txnQuery = @"INSERT INTO Transactions 
                                            (AccountNumber, TransactionDate, TransactionType, Amount, Description)
                                            VALUES (@AccountNumber, @TransactionDate, 'Interest', @Amount, @Description)";

                                SqlCommand txnCmd = new SqlCommand(txnQuery, connection, transaction);
                                txnCmd.Parameters.AddWithValue("@AccountNumber", accountNumber);
                                txnCmd.Parameters.AddWithValue("@TransactionDate", DateTime.Now);
                                txnCmd.Parameters.AddWithValue("@Amount", interest);
                                txnCmd.Parameters.AddWithValue("@Description", $"Interest credit {interest:C}");

                                txnCmd.ExecuteNonQuery();


                            }
                        }
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
    */
