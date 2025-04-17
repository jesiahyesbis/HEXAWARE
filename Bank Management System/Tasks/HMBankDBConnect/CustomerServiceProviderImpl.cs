using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
namespace HMBankDBConnect
{
    public class CustomerServiceProviderImpl : ICustomerServiceProvider
    {
        protected List<Account> accountList;
        protected List<Transaction> transactionList;

        public CustomerServiceProviderImpl()
        {
            accountList = new List<Account>();
            transactionList = new List<Transaction>();
        }

        public virtual decimal GetAccountBalance(long accountNumber)
        {
            Account account = accountList.Find(a => a.AccountNumber == accountNumber);
            if (account == null)
                throw new ArgumentException("Account not found.");

            return account.Balance;
        }

        public virtual decimal Deposit(long accountNumber, decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Deposit amount must be positive.");

            Account account = accountList.Find(a => a.AccountNumber == accountNumber);
            if (account == null)
                throw new ArgumentException("Account not found.");

            account.Balance += amount;
            transactionList.Add(new Transaction(account, "Deposit", "Deposit", amount));
            return account.Balance;
        }

        public virtual decimal Withdraw(long accountNumber, decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Withdrawal amount must be positive.");

            Account account = accountList.Find(a => a.AccountNumber == accountNumber);
            if (account == null)
                throw new ArgumentException("Account not found.");

            if (account is SavingsAccount savingsAccount)
            {
                if (account.Balance - amount < 500)
                    throw new InvalidOperationException("Withdrawal would violate minimum balance rule for savings account.");
            }
            else if (account is CurrentAccount currentAccount)
            {
                if (account.Balance - amount < -currentAccount.OverdraftLimit)
                    throw new InvalidOperationException("Withdrawal would exceed overdraft limit.");
            }

            account.Balance -= amount;
            transactionList.Add(new Transaction(account, "Withdrawal", "Withdraw", amount));
            return account.Balance;
        }

        public virtual bool Transfer(long fromAccountNumber, long toAccountNumber, decimal amount)
        {
            Account fromAccount = accountList.Find(a => a.AccountNumber == fromAccountNumber);
            Account toAccount = accountList.Find(a => a.AccountNumber == toAccountNumber);

            if (fromAccount == null || toAccount == null)
                throw new ArgumentException("One or both accounts not found.");

            decimal fromBalanceAfterWithdrawal = fromAccount.Balance - amount;

            // Check withdrawal rules for fromAccount
            if (fromAccount is SavingsAccount savingsAccount)
            {
                if (fromBalanceAfterWithdrawal < 500)
                    throw new InvalidOperationException("Transfer would violate minimum balance rule for savings account.");
            }
            else if (fromAccount is CurrentAccount currentAccount)
            {
                if (fromBalanceAfterWithdrawal < -currentAccount.OverdraftLimit)
                    throw new InvalidOperationException("Transfer would exceed overdraft limit.");
            }

            // Perform transfer
            fromAccount.Balance -= amount;
            toAccount.Balance += amount;

            // Record transactions
            transactionList.Add(new Transaction(fromAccount, $"Transfer to {toAccountNumber}", "Transfer", amount));
            transactionList.Add(new Transaction(toAccount, $"Transfer from {fromAccountNumber}", "Transfer", amount));

            return true;
        }

        public virtual Account GetAccountDetails(long accountNumber)
        {
            Account account = accountList.Find(a => a.AccountNumber == accountNumber);
            if (account == null)
                throw new ArgumentException("Account not found.");

            return account;
        }

        public virtual List<Transaction> GetTransactions(long accountNumber, DateTime fromDate, DateTime toDate)
        {
            Account account = accountList.Find(a => a.AccountNumber == accountNumber);
            if (account == null)
                throw new ArgumentException("Account not found.");

            return transactionList.FindAll(t =>
                t.Account.AccountNumber == accountNumber &&
                t.TransactionDate >= fromDate &&
                t.TransactionDate <= toDate);
        }
    }
}






