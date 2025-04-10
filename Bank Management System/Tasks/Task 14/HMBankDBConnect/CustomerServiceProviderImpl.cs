using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMBankDBConnect
{
    public class CustomerServiceProviderImpl : ICustomerServiceProvider
    {
        protected List<Account> accounts = new List<Account>();
        protected List<Transaction> transactions = new List<Transaction>();

        public decimal GetAccountBalance(long accountNumber)
        {
            Account account = FindAccount(accountNumber);
            return account.Balance;
        }

        public void Deposit(long accountNumber, decimal amount)
        {
            Account account = FindAccount(accountNumber);
            account.Deposit(amount);

            transactions.Add(new Transaction(
                accountNumber,
                "Deposit",
                DateTime.Now,
                "Deposit",
                amount
            ));
        }

        public void Withdraw(long accountNumber, decimal amount)
        {
            Account account = FindAccount(accountNumber);
            account.Withdraw(amount);

            transactions.Add(new Transaction(
                accountNumber,
                "Withdrawal",
                DateTime.Now,
                "Withdrawal",
                amount
            ));
        }

        public void Transfer(long fromAccountNumber, long toAccountNumber, decimal amount)
        {
            Account fromAccount = FindAccount(fromAccountNumber);
            Account toAccount = FindAccount(toAccountNumber);

            fromAccount.Withdraw(amount);
            toAccount.Deposit(amount);

            transactions.Add(new Transaction(
                fromAccountNumber,
                $"Transfer to {toAccountNumber}",
                DateTime.Now,
                "Transfer",
                amount
            ));

            transactions.Add(new Transaction(
                toAccountNumber,
                $"Transfer from {fromAccountNumber}",
                DateTime.Now,
                "Transfer",
                amount
            ));
        }

        public Account GetAccountDetails(long accountNumber) => FindAccount(accountNumber);

        public List<Transaction> GetTransactions(long accountNumber, DateTime fromDate, DateTime toDate)
        {
            return transactions.Where(t =>
                t.AccountNumber == accountNumber &&
                t.TransactionDate >= fromDate &&
                t.TransactionDate <= toDate
            ).ToList();
        }

        protected Account FindAccount(long accountNumber)
        {
            Account account = accounts.FirstOrDefault(a => a.AccountNumber == accountNumber);
            if (account == null)
                throw new ArgumentException("Account not found");
            return account;
        }
    }

}
