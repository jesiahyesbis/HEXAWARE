using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMBankDBConnect
{
    public interface IBankRepository
    {
        void CreateAccount(Customer customer, string accType, decimal balance);
        List<Account> ListAccounts();
        decimal GetAccountBalance(long accountNumber);
        void Deposit(long accountNumber, decimal amount);
        void Withdraw(long accountNumber, decimal amount);
        void Transfer(long fromAccountNumber, long toAccountNumber, decimal amount);
        Account GetAccountDetails(long accountNumber);
        List<Transaction> GetTransactions(long accountNumber, DateTime fromDate, DateTime toDate);
        void CalculateInterest();
    }

}
