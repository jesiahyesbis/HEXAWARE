using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMBankDBConnect
{
    public interface IBankServiceProvider
    {
        void CreateAccount(Customer customer, string accType, decimal balance);
        List<Account> ListAccounts();
        void CalculateInterest();
    }
}
