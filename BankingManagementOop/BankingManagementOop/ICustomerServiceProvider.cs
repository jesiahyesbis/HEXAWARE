using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingManagementOop
{
    internal interface ICustomerServiceProvider
    {
        double GetAccountBalance(long accountNumber);    // Retrieve the balance of an account.
        double Deposit(long accountNumber, double amount);    // Deposit the specified amount into the account.
        double Withdraw(long accountNumber, double amount);    // Withdraw the specified amount from the account.
        void Transfer(long fromAccountNumber, long toAccountNumber, double amount);    // Transfer money between accounts.
        string GetAccountDetails(long accountNumber);    // Get the account and customer details.
    }
}
