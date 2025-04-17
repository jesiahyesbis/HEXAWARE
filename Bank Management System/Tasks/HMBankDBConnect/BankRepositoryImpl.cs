using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace HMBankDBConnect
{
    public class BankRepositoryImpl : IBankRepository
    {
        public Account CreateAccount(Customer customer, long accNo, string accType, decimal balance)
        {
            using (SqlConnection connection = DBUtil.GetDBConn())
            {
                // inserting  the customer if they don't exist
                if (customer.CustomerID == 0)
                {
                    string insertCustomerQuery = @"
                INSERT INTO Customers (FirstName, LastName, EmailAddress, PhoneNumber, Address)
                VALUES (@FirstName, @LastName, @Email, @Phone, @Address);
                SELECT SCOPE_IDENTITY();";

                    using (SqlCommand command = new SqlCommand(insertCustomerQuery, connection))
                    {
                        command.Parameters.AddWithValue("@FirstName", customer.FirstName);
                        command.Parameters.AddWithValue("@LastName", customer.LastName);
                        command.Parameters.AddWithValue("@Email", customer.EmailAddress);
                        command.Parameters.AddWithValue("@Phone", customer.PhoneNumber);
                        command.Parameters.AddWithValue("@Address", customer.Address);

                        customer.CustomerID = Convert.ToInt32(command.ExecuteScalar());
                    }
                }

                // Then insert the account
                string insertAccountQuery = @"
            INSERT INTO Accounts (AccountNumber, AccountType, Balance, CustomerID, InterestRate, OverdraftLimit)
            VALUES (@AccNo, @AccType, @Balance, @CustomerID, @InterestRate, @OverdraftLimit)";

                using (SqlCommand command = new SqlCommand(insertAccountQuery, connection))
                {
                    command.Parameters.AddWithValue("@AccNo", accNo);
                    command.Parameters.AddWithValue("@AccType", accType);
                    command.Parameters.AddWithValue("@Balance", balance);
                    command.Parameters.AddWithValue("@CustomerID", customer.CustomerID);

                    if (accType == "Savings")
                    {
                        command.Parameters.AddWithValue("@InterestRate", 4.0m); 
                        command.Parameters.AddWithValue("@OverdraftLimit", DBNull.Value);
                    }
                    else if (accType == "Current")
                    {
                        command.Parameters.AddWithValue("@InterestRate", DBNull.Value);
                        command.Parameters.AddWithValue("@OverdraftLimit", 10000m); 
                    }
                    else // ZeroBalance
                    {
                        command.Parameters.AddWithValue("@InterestRate", DBNull.Value);
                        command.Parameters.AddWithValue("@OverdraftLimit", DBNull.Value);
                    }

                    command.ExecuteNonQuery();
                }

                return GetAccountDetails(accNo);
            }
        }

        public List<Account> ListAccounts()
        {
            List<Account> accounts = new List<Account>();

            using (SqlConnection connection = DBUtil.GetDBConn())
            {
                string query = @"
            SELECT a.AccountNumber, a.AccountType, a.Balance, a.InterestRate, a.OverdraftLimit,
                   c.CustomerID, c.FirstName, c.LastName, c.EmailAddress, c.PhoneNumber, c.Address
            FROM Accounts a
            JOIN Customers c ON a.CustomerID = c.CustomerID";

                using (SqlCommand command = new SqlCommand(query, connection))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Customer customer = new Customer(
                            reader.GetInt32(5),
                            reader.GetString(6),
                            reader.GetString(7),
                            reader.GetString(8),
                            reader.GetString(9),
                            reader.GetString(10));

                        Account account = null;
                        string accountType = reader.GetString(1);
                        long accountNumber = reader.GetInt64(0);
                        decimal balance = reader.GetDecimal(2);

                        switch (accountType)
                        {
                            case "Savings":
                                decimal interestRate = reader.IsDBNull(3) ? 0 : reader.GetDecimal(3);
                                account = new SavingsAccount(customer, balance, interestRate);
                                break;
                            case "Current":
                                decimal overdraftLimit = reader.IsDBNull(4) ? 0 : reader.GetDecimal(4);
                                account = new CurrentAccount(customer, balance, overdraftLimit);
                                break;
                            case "ZeroBalance":
                                account = new ZeroBalanceAccount(customer);
                                break;
                        }

                        if (account != null)
                        {
                            accounts.Add(account);
                        }
                    }
                }
            }

            return accounts;
        }

        public void CalculateInterest()
        {
            using (SqlConnection connection = DBUtil.GetDBConn())
            {
                string query = @"
            UPDATE Accounts
            SET Balance = Balance + (Balance * InterestRate / 100)
            WHERE AccountType = 'Savings' AND InterestRate IS NOT NULL";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }

                // Record interest transactions
                query = @"
            INSERT INTO Transactions (AccountNumber, Description, TransactionType, Amount)
            SELECT AccountNumber, 'Interest Credit', 'Deposit', Balance * InterestRate / 100
            FROM Accounts
            WHERE AccountType = 'Savings' AND InterestRate IS NOT NULL";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public decimal GetAccountBalance(long accountNumber)
        {
            using (SqlConnection connection = DBUtil.GetDBConn())
            {
                string query = "SELECT Balance FROM Accounts WHERE AccountNumber = @AccountNumber";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@AccountNumber", accountNumber);
                    object result = command.ExecuteScalar();
                    if (result == null)
                        throw new ArgumentException("Account not found.");

                    return Convert.ToDecimal(result);
                }
            }
        }

        public decimal Deposit(long accountNumber, decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Deposit amount must be positive.");

            using (SqlConnection connection = DBUtil.GetDBConn())
            {
                // Update balance
                string updateQuery = @"
            UPDATE Accounts 
            SET Balance = Balance + @Amount 
            WHERE AccountNumber = @AccountNumber";

                using (SqlCommand command = new SqlCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@AccountNumber", accountNumber);
                    command.Parameters.AddWithValue("@Amount", amount);
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected == 0)
                        throw new ArgumentException("Account not found.");
                }

                // Record transaction
                string insertQuery = @"
            INSERT INTO Transactions (AccountNumber, Description, TransactionType, Amount)
            VALUES (@AccountNumber, 'Deposit', 'Deposit', @Amount)";

                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@AccountNumber", accountNumber);
                    command.Parameters.AddWithValue("@Amount", amount);
                    command.ExecuteNonQuery();
                }

                return GetAccountBalance(accountNumber);
            }
        }

        public decimal Withdraw(long accountNumber, decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Withdrawal amount must be positive.");

            using (SqlConnection connection = DBUtil.GetDBConn())
            {
                // First get account details to check withdrawal rules
                Account account = GetAccountDetails(accountNumber);
                decimal newBalance = account.Balance - amount;

                // Check withdrawal rules
                if (account is SavingsAccount)
                {
                    if (newBalance < 500)
                        throw new InvalidOperationException("Withdrawal would violate minimum balance rule for savings account.");
                }
                else if (account is CurrentAccount currentAccount)
                {
                    if (newBalance < -currentAccount.OverdraftLimit)
                        throw new InvalidOperationException("Withdrawal would exceed overdraft limit.");
                }

                // Update balance
                string updateQuery = @"
            UPDATE Accounts 
            SET Balance = Balance - @Amount 
            WHERE AccountNumber = @AccountNumber";

                using (SqlCommand command = new SqlCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@AccountNumber", accountNumber);
                    command.Parameters.AddWithValue("@Amount", amount);
                    command.ExecuteNonQuery();
                }

                // Record transaction
                string insertQuery = @"
            INSERT INTO Transactions (AccountNumber, Description, TransactionType, Amount)
            VALUES (@AccountNumber, 'Withdrawal', 'Withdraw', @Amount)";

                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@AccountNumber", accountNumber);
                    command.Parameters.AddWithValue("@Amount", amount);
                    command.ExecuteNonQuery();
                }

                return GetAccountBalance(accountNumber);
            }
        }

        public bool Transfer(long fromAccountNumber, long toAccountNumber, decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Transfer amount must be positive.");

            using (SqlConnection connection = DBUtil.GetDBConn())
            {
                SqlTransaction transaction = connection.BeginTransaction();

                try
                {
                    // Get both accounts
                    Account fromAccount = GetAccountDetails(fromAccountNumber);
                    Account toAccount = GetAccountDetails(toAccountNumber);

                    // Check withdrawal rules for fromAccount
                    decimal fromBalanceAfterWithdrawal = fromAccount.Balance - amount;

                    if (fromAccount is SavingsAccount)
                    {
                        if (fromBalanceAfterWithdrawal < 500)
                            throw new InvalidOperationException("Transfer would violate minimum balance rule for savings account.");
                    }
                    else if (fromAccount is CurrentAccount currentAccount)
                    {
                        if (fromBalanceAfterWithdrawal < -currentAccount.OverdraftLimit)
                            throw new InvalidOperationException("Transfer would exceed overdraft limit.");
                    }

                    // Perform withdrawal from source account
                    string withdrawQuery = @"
                UPDATE Accounts 
                SET Balance = Balance - @Amount 
                WHERE AccountNumber = @FromAccountNumber";

                    using (SqlCommand command = new SqlCommand(withdrawQuery, connection, transaction))
                    {
                        command.Parameters.AddWithValue("@FromAccountNumber", fromAccountNumber);
                        command.Parameters.AddWithValue("@Amount", amount);
                        command.ExecuteNonQuery();
                    }

                    // Record withdrawal transaction
                    string withdrawTransQuery = @"
                INSERT INTO Transactions (AccountNumber, Description, TransactionType, Amount)
                VALUES (@FromAccountNumber, @Description, 'Transfer', @Amount)";

                    using (SqlCommand command = new SqlCommand(withdrawTransQuery, connection, transaction))
                    {
                        command.Parameters.AddWithValue("@FromAccountNumber", fromAccountNumber);
                        command.Parameters.AddWithValue("@Description", $"Transfer to {toAccountNumber}");
                        command.Parameters.AddWithValue("@Amount", amount);
                        command.ExecuteNonQuery();
                    }

                    // Perform deposit to target account
                    string depositQuery = @"
                UPDATE Accounts 
                SET Balance = Balance + @Amount 
                WHERE AccountNumber = @ToAccountNumber";

                    using (SqlCommand command = new SqlCommand(depositQuery, connection, transaction))
                    {
                        command.Parameters.AddWithValue("@ToAccountNumber", toAccountNumber);
                        command.Parameters.AddWithValue("@Amount", amount);
                        command.ExecuteNonQuery();
                    }

                    // Record deposit transaction
                    string depositTransQuery = @"
                INSERT INTO Transactions (AccountNumber, Description, TransactionType, Amount)
                VALUES (@ToAccountNumber, @Description, 'Transfer', @Amount)";

                    using (SqlCommand command = new SqlCommand(depositTransQuery, connection, transaction))
                    {
                        command.Parameters.AddWithValue("@ToAccountNumber", toAccountNumber);
                        command.Parameters.AddWithValue("@Description", $"Transfer from {fromAccountNumber}");
                        command.Parameters.AddWithValue("@Amount", amount);
                        command.ExecuteNonQuery();
                    }

                    transaction.Commit();
                    return true;
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
            using (SqlConnection connection = DBUtil.GetDBConn())
            {
                string query = @"
            SELECT a.AccountNumber, a.AccountType, a.Balance, a.InterestRate, a.OverdraftLimit,
                   c.CustomerID, c.FirstName, c.LastName, c.EmailAddress, c.PhoneNumber, c.Address
            FROM Accounts a
            JOIN Customers c ON a.CustomerID = c.CustomerID
            WHERE a.AccountNumber = @AccountNumber";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@AccountNumber", accountNumber);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (!reader.Read())
                            throw new ArgumentException("Account not found.");

                        Customer customer = new Customer(
                            reader.GetInt32(5),
                            reader.GetString(6),
                            reader.GetString(7),
                            reader.GetString(8),
                            reader.GetString(9),
                            reader.GetString(10));

                        Account account = null;
                        string accountType = reader.GetString(1);
                        decimal balance = reader.GetDecimal(2);

                        switch (accountType)
                        {
                            case "Savings":
                                decimal interestRate = reader.IsDBNull(3) ? 0 : reader.GetDecimal(3);
                                account = new SavingsAccount(customer, balance, interestRate);
                                break;
                            case "Current":
                                decimal overdraftLimit = reader.IsDBNull(4) ? 0 : reader.GetDecimal(4);
                                account = new CurrentAccount(customer, balance, overdraftLimit);
                                break;
                            case "ZeroBalance":
                                account = new ZeroBalanceAccount(customer);
                                break;
                        }

                        return account;
                    }
                }
            }
        }

        public List<Transaction> GetTransactions(long accountNumber, DateTime fromDate, DateTime toDate)
        {
            List<Transaction> transactions = new List<Transaction>();

            using (SqlConnection connection = DBUtil.GetDBConn())
            {
                string query = @"
            SELECT t.TransactionID, t.Description, t.TransactionDate, t.TransactionType, t.Amount,
                   a.AccountNumber, a.AccountType, a.Balance,
                   c.CustomerID, c.FirstName, c.LastName, c.EmailAddress, c.PhoneNumber, c.Address
            FROM Transactions t
            JOIN Accounts a ON t.AccountNumber = a.AccountNumber
            JOIN Customers c ON a.CustomerID = c.CustomerID
            WHERE t.AccountNumber = @AccountNumber 
            AND t.TransactionDate BETWEEN @FromDate AND @ToDate
            ORDER BY t.TransactionDate";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@AccountNumber", accountNumber);
                    command.Parameters.AddWithValue("@FromDate", fromDate);
                    command.Parameters.AddWithValue("@ToDate", toDate);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Customer customer = new Customer(
                                reader.GetInt32(8),
                                reader.GetString(9),
                                reader.GetString(10),
                                reader.GetString(11),
                                reader.GetString(12),
                                reader.GetString(13));

                            Account account = null;
                            string accountType = reader.GetString(6);
                            decimal balance = reader.GetDecimal(7);

                            switch (accountType)
                            {
                                case "Savings":
                                    account = new SavingsAccount(customer, balance, 0); // Interest rate not needed here
                                    break;
                                case "Current":
                                    account = new CurrentAccount(customer, balance, 0); // Overdraft not needed here
                                    break;
                                case "ZeroBalance":
                                    account = new ZeroBalanceAccount(customer);
                                    break;
                            }

                            if (account != null)
                            {
                                account.AccountNumber = reader.GetInt64(5);
                                Transaction transaction = new Transaction(
                                    account,
                                    reader.GetString(1),
                                    reader.GetString(3),
                                    reader.GetDecimal(4))
                                {
                                    TransactionID = reader.GetInt32(0),
                                    TransactionDate = reader.GetDateTime(2)
                                };
                                transactions.Add(transaction);
                            }
                        }
                    }
                }
            }

            return transactions;
        }
    }
}
