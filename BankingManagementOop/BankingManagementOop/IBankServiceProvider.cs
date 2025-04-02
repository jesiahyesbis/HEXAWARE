using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingManagementOop
{
    internal interface IBankServiceProvider : ICustomerServiceProvider
    {
        void CreateAccount(Customer customer, long accNo, string accType, double balance);    // Create a new bank account.
        List<Account> ListAccounts();    // List all accounts.
        void CalculateInterest(long accountNumber);    // Calculate interest for a specific account
    }
}
