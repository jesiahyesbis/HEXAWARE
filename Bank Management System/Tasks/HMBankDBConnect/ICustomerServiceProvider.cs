using System;
using System.Collections.Generic;
namespace HMBankDBConnect
{
    public interface ICustomerServiceProvider
    {
        decimal GetAccountBalance(long accountNumber);
        decimal Deposit(long accountNumber, decimal amount);
        decimal Withdraw(long accountNumber, decimal amount);
        bool Transfer(long fromAccountNumber, long toAccountNumber, decimal amount);
        Account GetAccountDetails(long accountNumber);
        List<Transaction> GetTransactions(long accountNumber, DateTime fromDate, DateTime toDate);
    }
}



